using MongoDB.Bson;
using System;

namespace MongoGogo.Connection
{
    /// <summary>
    /// The underlying implementation of GoCollectionObserver uses MongoDB change streams. 
    /// It will notify subscribers when the collection undergoes insertions, deletions, updates, or replacements
    /// </summary>
    /// <typeparam name="TDocument"> mongo document</typeparam>
    public interface IGoCollectionObserver<TDocument>
    {
        /// <summary>
        /// Subscribe to the insert event with the specified action. The action will be executed when a document is inserted.
        /// </summary>
        /// <param name="action">The action to be executed on insert events.</param>
        public void OnInsert(Action<TDocument> action);

        /// <summary>
        /// Subscribe to the update event with the specified action. The action will be executed when a document is updated.
        /// </summary>
        /// <param name="action">The action to be executed on update events.</param>
        public void OnUpdate(Action<TDocument> action);

        /// <summary>
        /// Subscribe to the replace event with the specified action. The action will be executed when a document is replaced.
        /// </summary>
        /// <param name="action">The action to be executed on replace events.</param>
        public void OnReplace(Action<TDocument> action);

        /// <summary>
        /// Subscribe to the delete event with the specified action. The action will be executed when a document is deleted.
        /// </summary>
        /// <param name="action">The action to be executed on delete events.</param>
        public void OnDelete(Action<ObjectId> action);

        /// <summary>
        /// Subscribe to any event with the specified action. The action will be executed when any event (insert, update, replace, delete) occurs.
        /// </summary>
        /// <param name="action">The action to be executed on any event.</param>
        public void OnAnyEvent(Action action);
    }
}
