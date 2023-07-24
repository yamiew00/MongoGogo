using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Collection observer that tracks changes in a MongoDB collection using MongoDB change streams.
    /// It notifies subscribers when the collection undergoes insertions, deletions, updates, or replacements.
    /// </summary>
    /// <typeparam name="TDocument"> mongo document</typeparam>
    public interface IGoCollectionObserver<TDocument>
    {
        /// <summary>
        /// Subscribe to the "INSERT" event with the specified action. 
        /// The action will be executed when a document is inserted.
        /// </summary>
        /// <param name="action">The action to be executed when a document is inserted.</param>
        public void OnInsert(Action<TDocument> action);

        /// <summary>
        /// Subscribe to the "UPDATE" event with the specified action. 
        /// The action will be executed when a document is updated.
        /// </summary>
        /// <param name="action">The action to be executed when a document is updated.</param>
        public void OnUpdate(Action<TDocument> action);

        /// <summary>
        /// Subscribe to the "REPLACE" event with the specified action. 
        /// The action will be executed when a document is replaced.
        /// </summary>
        /// <param name="action">The action to be executed when a document is replaced.</param>
        public void OnReplace(Action<TDocument> action);

        /// <summary>
        /// Subscribe to the "DELETE" event with the specified action. 
        /// This method is intended for documents with an _id field of type ObjectId or GoDocument. 
        /// If Bson of TDocument is of type other than "ObjectId", use OnDelete<TBsonIdType> instead.
        /// The action will be executed when such a document is deleted.
        /// </summary>
        /// <param name="action">The action to be executed when a document is deleted.</param>
        /// <remarks>
        /// Use this method if your document's _id is of type ObjectId (default) or GoDocument. 
        /// For other _id field types, use OnDelete<TBsonType> method.
        /// </remarks>
        public void OnDelete(Action<ObjectId> action);

        /// <summary>
        /// Subscribe to the "DELETE" event with the specified action and the specified BSON type for the _id field. 
        /// This method allows for flexibility when the _id field 
        /// The action will be executed when a document is deleted. 
        /// </summary>
        /// <typeparam name="TBsonIdType">The BSON type of the _id field in the documents.</typeparam>
        /// <param name="action">The action to be executed when a document is deleted.</param>
        /// <remarks>
        /// This method provides flexibility when the _id field of your documents is not of type ObjectId. 
        /// Use this method when you need to specify a different type for the _id field.
        /// </remarks>
        public void OnDelete<TBsonIdType>(Action<TBsonIdType> action);

        /// <summary>
        /// Subscribe to "ANY EVENT" with the specified action. 
        /// The action will be executed when any event (INSERT, UPDATE, REPLACE, DELETE) occurs.
        /// </summary>
        /// <param name="action">The action to be executed when one of INSERT, UPDATE, REPLACE or DELETE occurs.</param>
        public void OnAnyEvent(Action action);

        /// <summary>
        /// Subscribe to the "INSERT" event with the specified asynchronous task. 
        /// The task will be executed when a document is inserted.
        /// </summary>
        /// <param name="task">The asynchronous task to be executed when a document is inserted.</param>
        public void OnInsertAsync(Func<TDocument, Task> task);

        /// <summary>
        /// Subscribe to the "UPDATE" event with the specified asynchronous task. 
        /// The task will be executed when a document is updated.
        /// </summary>
        /// <param name="task">The asynchronous task to be executed when a document is updated.</param>
        public void OnUpdateAsync(Func<TDocument, Task> task);

        /// <summary>
        /// Subscribe to the "REPLACE" event with the specified asynchronous task. 
        /// The task will be executed when a document is replaced.
        /// </summary>
        /// <param name="task">The asynchronous task to be executed when a document is replaced.</param>
        public void OnReplaceAsync(Func<TDocument, Task> task);

        /// <summary>
        /// Subscribe to the "DELETE" event with the specified asynchronous task. 
        /// If Bson of TDocument is of type other than "ObjectId", use OnDeleteAsync<TBsonIdType> instead.
        /// The task will be executed when a document is deleted.
        /// </summary>
        /// <param name="task">The asynchronous task to be executed when a document is deleted.</param>
        public void OnDeleteAsync(Func<ObjectId, Task> task);

        /// <summary>
        /// Subscribe to the "DELETE" event with the specified asynchronous task and the specified BSON type for the _id field. 
        /// This method allows for flexibility when the _id field 
        /// The task will be executed when a document is deleted. 
        /// </summary>
        /// <typeparam name="TBsonIdType">The BSON type of the _id field in the documents.</typeparam>
        /// <param name="task">The asynchronous task to be executed when a document is deleted.</param>
        /// <remarks>
        /// This method provides flexibility when the _id field of your documents is not of type ObjectId. 
        /// Use this method when you need to specify a different type for the _id field.
        /// </remarks>
        public void OnDeleteAsync<TBsonIdType>(Func<TBsonIdType, Task> task);

        /// <summary>
        /// Subscribe to "ANY EVENT" with the specified asynchronous task. 
        /// The task will be executed when any event (INSERT, UPDATE, REPLACE, DELETE) occurs.
        /// </summary>
        /// <param name="task">The asynchronous task to be executed when one of INSERT, UPDATE, REPLACE, or DELETE occurs.</param>
        public void OnAnyEventAsync(Func<Task> task);
    }
}
