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

        private readonly List<Action<TDocument>> UpdateActions = new List<Action<TDocument>>();

        private readonly List<Action<TDocument>> ReplaceActions = new List<Action<TDocument>>();

        private readonly List<Action<ObjectId>> DeleteActions = new List<Action<ObjectId>>();

        private readonly Dictionary<Type, List<Action<object>>> DeleteGenericDictionary = new Dictionary<Type, List<Action<object>>>();
        private readonly Dictionary<Type, Type> GoGenericDocumentDictionary = new Dictionary<Type, Type>(); //dictionary for GoGenericDocument<type>, use this to avoid reflection waste.

        private readonly List<Action> AnyEventActions = new List<Action>();

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
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task WatchAsync()
        {
            var cursor = await MongoCollection.WatchAsync(new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup  //it will cause an instant BsonDocument result in ChangeStreamOperationType.Update
            });

            await cursor.ForEachAsync(change =>
            {
                //insert
                if (change.OperationType == ChangeStreamOperationType.Insert)
                {
                    TDocument insertValue = change.FullDocument;

                    foreach (var action in InsertActions)
                    {
                        action.Invoke(insertValue);
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
                }

                //replace
                if (change.OperationType == ChangeStreamOperationType.Replace)
                {
                    TDocument insertValue = change.FullDocument;

                    foreach (var action in ReplaceActions)
                    {
                        action.Invoke(insertValue);
                    }
                }

                //delete
                if (change.OperationType == ChangeStreamOperationType.Delete)
                {
                    if (DeleteActions.Any())
                    {
                        try
                        {
                            ObjectId _id = BsonSerializer.Deserialize<GoDocument>(change.DocumentKey)._id;
                            foreach (var action in DeleteActions)
                            {
                                action.Invoke(_id);
                            }
                        }
                        catch { }
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

                            try
                            {
                                var genericDocumentType = MakeGenericType(type);
                                var _id = ((dynamic)BsonSerializer.Deserialize(change.DocumentKey, genericDocumentType))._id;
                                foreach (var action in actionList)
                                {
                                    action.Invoke(_id);
                                }
                                break;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                //anyEvent: Run once regardless of the event that occurs.
                foreach (var anyEvent in AnyEventActions)
                {
                    anyEvent.Invoke();
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
    }
}
