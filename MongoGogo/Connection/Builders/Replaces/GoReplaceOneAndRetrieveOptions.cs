using MongoDB.Driver;
using MongoGogo.Connection.Builders.Finds;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection.Builders.Replaces
{
    /// <summary>
    ///  Options for a ReplaceOneAndRetrieve command to update an object.
    /// </summary>
    /// <typeparam name="TDocument">The Document type</typeparam>
    public class GoReplaceOneAndRetrieveOptions<TDocument>
    {
        /// <summary>
        /// The projection
        /// </summary>
        public Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> Projection { get; set; } = default;

        /// <summary>
        /// Which version of the document to return when executing a UpdateAndRetrieve command.
        /// </summary>
        public ReturnDocument ReturnDocument { get; set; } = ReturnDocument.Before;

        /// <summary>
        /// Gets or sets a value indicating whether to insert the document if it doesn't already exist.
        /// </summary>
        public bool IsUpsert { get; set; } = false;

        /// <summary>
        /// Defines the sorting order of the documents in the result set
        /// </summary>
        public Expression<Func<GoSortBuilder<TDocument>, GoSortDefinition<TDocument>>> Sort { get; set; } = default;
    }
}
