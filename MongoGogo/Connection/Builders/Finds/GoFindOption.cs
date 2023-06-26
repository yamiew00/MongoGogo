using MongoGogo.Connection.Builders.Finds;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Options for finding documents.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class GoFindOption<TDocument>
    {
        /// <summary>
        /// Set whether the server is allowed to write to disk while executing the Find operation.
        /// </summary>
        public bool AllowDiskUse { get; set; } = false;

        /// <summary>
        /// Limits the number of documents.
        /// </summary>
        /// <remarks>
        /// Limit of zero or null is equivalent to setting no limit.
        /// </remarks>
        public int? Limit { get; set; } = default;

        /// <summary>
        /// Skips the the specified number of documents.
        /// </summary>
        public int? Skip { get; set; } = default;

        /// <summary>
        /// Defines the sorting order of the documents in the result set
        /// </summary>
        public Expression<Func<GoSortBuilder<TDocument>, GoSortDefinition<TDocument>>> Sort { get; set; } = default;
    }
}
