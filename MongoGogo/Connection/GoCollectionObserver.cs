using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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
    public class GoCollectionObserver<TDocument> : IGoCollectionObserver<TDocument> where TDocument : GoDocument
    {
        private readonly IMongoCollection<TDocument> MongoCollection;

        private bool _hasSubscriber = false;

        private readonly List<Action<TDocument>> InsertActions = new List<Action<TDocument>>();

        private readonly List<Action<TDocument>> UpdateActions = new List<Action<TDocument>>();

        private readonly List<Action<TDocument>> ReplaceActions = new List<Action<TDocument>>();

        private readonly List<Action<ObjectId>> DeleteActions = new List<Action<ObjectId>>();

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
            var cursor = await MongoCollection.WatchAsync();

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
                    //The updated value can only be found in the database after the change, as the change only carries the key, not all the data.
                    var key = BsonSerializer.Deserialize<TDocument>(change.DocumentKey);
                    var updateValue = MongoCollection.Find<TDocument>(x => x._id == key._id).Limit(1).ToList().FirstOrDefault();

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
                    ObjectId _id = BsonSerializer.Deserialize<TDocument>(change.DocumentKey)._id;
                    foreach (var action in DeleteActions)
                    {
                        action.Invoke(_id);
                    }
                }

                //anyEvent: Run once regardless of the event that occurs.
                foreach (var anyEvent in AnyEventActions)
                {
                    anyEvent.Invoke();
                }
            });
        }
    }
}
