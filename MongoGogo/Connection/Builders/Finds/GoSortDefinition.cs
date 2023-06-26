using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MongoGogo.Connection.Builders.Finds
{
    /// <summary>
    /// Defines a sort operation on a collection.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public class GoSortDefinition<TDocument>
    {
        /// <summary>
        /// The primary rule used for sorting.
        /// </summary>
        internal GoSortRule<TDocument> _primarySortRule { get; private set; }

        /// <summary>
        /// Additional rules used for sorting when values compared by the primary rule are equal.
        /// </summary>
        internal List<GoSortRule<TDocument>> _secondarySortRules { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GoSortDefinition class with a primary sort rule.
        /// </summary>
        /// <param name="orderType">The sort order type.</param>
        /// <param name="keySelector">The key selector function.</param>
        internal GoSortDefinition(OrderType orderType, Expression<Func<TDocument, object>> keySelector)
        {
            this._primarySortRule = new GoSortRule<TDocument>(orderType, keySelector);

            _secondarySortRules = new List<GoSortRule<TDocument>>();
        }

        /// <summary>
        /// Adds an additional sort rule in ascending order.
        /// </summary>
        /// <param name="keySelector">The key selector function for the additional sort rule.</param>
        /// <returns>The GoSortDefinition instance for method chaining.</returns>
        public GoSortDefinition<TDocument> ThenBy(Expression<Func<TDocument, object>> keySelector)
        {
            _secondarySortRules.Add(new GoSortRule<TDocument>(OrderType.Ascending, keySelector));
            return this;
        }

        /// <summary>
        /// Adds an additional sort rule in descending order.
        /// </summary>
        /// <param name="keySelector">The key selector function for the additional sort rule.</param>
        /// <returns>The GoSortDefinition instance for method chaining.</returns>
        public GoSortDefinition<TDocument> ThenByDescending(Expression<Func<TDocument, object>> keySelector)
        {
            _secondarySortRules.Add(new GoSortRule<TDocument>(OrderType.Descending, keySelector));
            return this;
        }
    }

    internal enum OrderType
    {
        Ascending,  
        Descending  
    }
}
