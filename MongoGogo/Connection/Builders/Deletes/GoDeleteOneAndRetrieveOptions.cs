using System.Linq.Expressions;
using System;
using MongoGogo.Connection.Builders.Finds;

namespace MongoGogo.Connection.Builders.Deletes
{
    /// <summary>
    ///  Options for a DeleteOneAndRetrieve command to update an object.
    /// </summary>
    /// <typeparam name="TDocument">The Document type</typeparam>
    public class GoDeleteOneAndRetrieveOptions<TDocument>
    {
        /// <summary>
        /// The projection
        /// </summary>
        public Expression<Func<GoProjectionBuilder<TDocument>, GoProjectionDefinition<TDocument>>> Projection { get; set; } = default;

        /// <summary>
        /// Defines the sorting order of the documents in the result set
        /// </summary>
        public Expression<Func<GoSortBuilder<TDocument>, GoSortDefinition<TDocument>>> Sort { get; set; } = default;
    }
}
