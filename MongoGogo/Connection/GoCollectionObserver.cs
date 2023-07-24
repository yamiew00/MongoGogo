using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoGogo.Connection.ObserverItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// The underlying implementation of GoCollectionObserver uses MongoDB change streams. 
    /// It will notify subscribers when the collection undergoes insertions, deletions, updates, or replacements
    /// </summary>
    /// <typeparam name="TDocument"> mongo document</typeparam>
    internal class GoCollectionObserver<TDocument> : IGoCollectionObserver<TDocument>
    {
        private readonly IMongoCollection<TDocument> MongoCollection;

        private bool _hasSubscriber = false;

        private readonly List<Action<TDocument>> InsertActions = new List<Action<TDocument>>();
        private readonly List<Func<TDocument, Task>> InsertTasks = new List<Func<TDocument, Task>>();

        private readonly List<Action<TDocument>> UpdateActions = new List<Action<TDocument>>();
        private readonly List<Func<TDocument, Task>> UpdateTasks = new List<Func<TDocument, Task>>();

        private readonly List<Action<TDocument>> ReplaceActions = new List<Action<TDocument>>();
        private readonly List<Func<TDocument, Task>> ReplaceTasks = new List<Func<TDocument, Task>>();

        private readonly List<Action<ObjectId>> DeleteActions = new List<Action<ObjectId>>();
        private readonly List<Func<ObjectId, Task>> DeleteTasks = new List<Func<ObjectId, Task>>();

        private readonly Dictionary<Type, List<Action<object>>> DeleteGenericDictionary = new Dictionary<Type, List<Action<object>>>();
        private readonly Dictionary<Type, List<Func<object, Task>>> DeleteGenericDictionaryAsync = new Dictionary<Type, List<Func<object, Task>>>();
        private readonly Dictionary<Type, Type> GoGenericDocumentDictionary = new Dictionary<Type, Type>(); //dictionary for GoGenericDocument<type>, use this to avoid reflection waste.

        private readonly List<Action> AnyEventActions = new List<Action>();
        private readonly List<Func<Task>> AnyEventTasks = new List<Func<Task>>();

        public GoCollectionObserver(IMongoCollection<TDocument> mongoCollection)
        {
            MongoCollection = mongoCollection;
        }

        public void OnInsert(Action<TDocument> action)
        {
            InsertActions.Add(action);
            TryStartObserveIfNotSubscribed();
        }

        public void OnUpdate(Action<TDocument> action)
        {
            UpdateActions.Add(action);
            TryStartObserveIfNotSubscribed();
        }

        public void OnReplace(Action<TDocument> action)
        {
            ReplaceActions.Add(action);
            TryStartObserveIfNotSubscribed();
        }

        public void OnDelete(Action<ObjectId> action)
        {
            DeleteActions.Add(action);
            TryStartObserveIfNotSubscribed();
        }

        public void OnDelete<TBsonIdType>(Action<TBsonIdType> action)
        {
            var bsonIdType = typeof(TBsonIdType);
            if (!DeleteGenericDictionary.TryGetValue(bsonIdType, out var _)) DeleteGenericDictionary[bsonIdType] = new List<Action<object>>();

            // Convert the action to an Action<object>
            Action<object> objectAction = obj => action((TBsonIdType)obj);

            DeleteGenericDictionary[bsonIdType].Add(objectAction);
            TryStartObserveIfNotSubscribed();
        }

        public void OnAnyEvent(Action action)
        {
            AnyEventActions.Add(action);
            TryStartObserveIfNotSubscribed();
        }

        /// <summary>
        /// The method 'StartOberserve' should be executed only once.
        /// </summary>
        private void TryStartObserveIfNotSubscribed()
        {
            if (!_hasSubscriber)
            {
                _hasSubscriber = true;
                StartOberserve();
            }
        }

        /// <summary>
        /// start the cursor of mongo change stream. This method never stops.
        /// </summary>
        /// <returns></returns>
        private async Task StartOberserve()
        {
            while (true)
            {
                try
                {
                    await WatchAsync();
                }
                catch
                {
                    //ignore
                }
            }
        }

        private async Task WatchAsync()
        {
            var cursor = await MongoCollection.WatchAsync(new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup  //it will cause an instant BsonDocument result in ChangeStreamOperationType.Update
            });

            await cursor.ForEachAsync(async change =>
            {
                //insert
                if (change.OperationType == ChangeStreamOperationType.Insert)
                {
                    TDocument insertValue = change.FullDocument;

                    foreach (var action in InsertActions)
                    {
                        action.Invoke(insertValue);
                    }

                    foreach (var task in InsertTasks)
                    {
                        await task(insertValue);
                    }
                }

                //update
                if (change.OperationType == ChangeStreamOperationType.Update)
                {
                    TDocument updateValue = change.FullDocument;

                    foreach (var action in UpdateActions)
                    {
                        action.Invoke(updateValue);
                    }

                    foreach (var task in UpdateTasks)
                    {
                        await task(updateValue);
                    }
                }

                //replace
                if (change.OperationType == ChangeStreamOperationType.Replace)
                {
                    TDocument replaceValue = change.FullDocument;

                    foreach (var action in ReplaceActions)
                    {
                        action.Invoke(replaceValue);
                    }

                    foreach (var task in ReplaceTasks)
                    {
                        await task(replaceValue);
                    }
                }

                //delete
                if (change.OperationType == ChangeStreamOperationType.Delete)
                {
                    if (DeleteActions.Any())
                    {
                        ObjectId _id = BsonSerializer.Deserialize<GoDocument>(change.DocumentKey)._id;
                        foreach (var action in DeleteActions)
                        {
                            action.Invoke(_id);
                        }

                        foreach (var task in DeleteTasks)
                        {
                            await task(_id);
                        }
                    }

                    // Loop through the DeleteGenericDictionary because the type of change.DocumentKey is not guaranteed.
                    // We rely on the types provided by the user (which are stored as keys in DeleteGenericDictionary) to deserialize it.
                    // If deserialization for a particular type fails, the exception is silently ignored and the next type is tried.
                    // This ensures that we can handle the delete action for the correct type.
                    // Once a successful deserialization occurs, the corresponding actions are invoked and the loop is exited.
                    if (DeleteGenericDictionary.Any())
                    {
                        foreach (var type_ActionList in DeleteGenericDictionary)
                        {
                            var type = type_ActionList.Key;
                            var actionList = type_ActionList.Value;

                            var genericDocumentType = MakeGenericType(type);
                            var _id = ((dynamic)BsonSerializer.Deserialize(change.DocumentKey, genericDocumentType))._id;
                            foreach (var action in actionList)
                            {
                                action.Invoke(_id);
                            }
                        }
                    }

                    if (DeleteGenericDictionaryAsync.Any())
                    {
                        foreach (var type_taskList in DeleteGenericDictionaryAsync)
                        {
                            var type = type_taskList.Key;
                            var taskList = type_taskList.Value;

                            var genericDocumentType = MakeGenericType(type);
                            var _id = ((dynamic)BsonSerializer.Deserialize(change.DocumentKey, genericDocumentType))._id;
                            foreach (var task in taskList)
                            {
                                await task(_id);
                            }
                        }
                    }
                }

                //anyEvent: Run once regardless of the event that occurs.
                foreach (var anyEvent in AnyEventActions)
                {
                    anyEvent.Invoke();
                }

                //anyEvent: Run once regardless of the event that occurs.
                foreach (var task in AnyEventTasks)
                {
                    await task.Invoke();
                }
            });
        }

        /// <summary>
        /// Uses cache to get genericType as possible to avoid using reflection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type MakeGenericType(Type type)
        {
            if (!GoGenericDocumentDictionary.TryGetValue(type, out var genericDocument))
            {
                genericDocument = typeof(GoGenericDocument<>).MakeGenericType(type);
                GoGenericDocumentDictionary.Add(type, genericDocument);
            }
            return genericDocument;
        }

        public void OnInsertAsync(Func<TDocument, Task> task)
        {
            InsertTasks.Add(task);
            TryStartObserveIfNotSubscribed();
        }

        public void OnUpdateAsync(Func<TDocument, Task> task)
        {
            UpdateTasks.Add(task);
            TryStartObserveIfNotSubscribed();
        }

        public void OnReplaceAsync(Func<TDocument, Task> task)
        {
            ReplaceTasks.Add(task);
            TryStartObserveIfNotSubscribed();
        }

        public void OnDeleteAsync(Func<ObjectId, Task> task)
        {
            DeleteTasks.Add(task);
            TryStartObserveIfNotSubscribed();
        }

        public void OnDeleteAsync<TBsonIdType>(Func<TBsonIdType, Task> task)
        {
            var bsonIdType = typeof(TBsonIdType);
            if (!DeleteGenericDictionaryAsync.TryGetValue(bsonIdType, out var _)) DeleteGenericDictionaryAsync[bsonIdType] = new List<Func<object, Task>>();

            // Convert the action to an Action<object>
            Func<object, Task> objectTask = obj => task((TBsonIdType)obj);

            DeleteGenericDictionaryAsync[bsonIdType].Add(objectTask);
            TryStartObserveIfNotSubscribed();
        }

        public void OnAnyEventAsync(Func<Task> task)
        {
            AnyEventTasks.Add(task);
            TryStartObserveIfNotSubscribed();
        }
    }
}
