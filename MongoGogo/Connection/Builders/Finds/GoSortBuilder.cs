using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection.Builders.Finds
{
    /// <summary>
    /// Constructs sort definitions for documents of type TDocument.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public class GoSortBuilder<TDocument>
    {
        internal GoSortBuilder() {}

        /// <summary>
        /// Sorts elements in ascending order.
        /// </summary>
        /// <param name="keySelector">The key selector function for the sort rule.</param>
        /// <returns>A GoSortDefinition instance for method chaining.</returns>
        public GoSortDefinition<TDocument> OrderBy(Expression<Func<TDocument, object>> keySelector)
        {
            return new GoSortDefinition<TDocument>(OrderType.Ascending, keySelector);
        }

        /// <summary>
        /// Sorts elements in descending order.
        /// </summary>
        /// <param name="keySelector">The key selector function for the sort rule.</param>
        /// <returns>A GoSortDefinition instance for method chaining.</returns>
        public GoSortDefinition<TDocument> OrderByDescending(Expression<Func<TDocument, object>> keySelector)
        {
            return new GoSortDefinition<TDocument>(OrderType.Descending, keySelector);
        }
    }
}
