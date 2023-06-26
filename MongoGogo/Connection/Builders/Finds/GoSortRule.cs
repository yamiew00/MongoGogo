using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection.Builders.Finds
{
    /// <summary>
    /// Represents a sorting rule for sorting documents in a MongoDB collection.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    internal class GoSortRule<TDocument>
    {
        /// <summary>
        /// The order type (ascending or descending) for this sorting rule.
        /// </summary>
        internal OrderType OrderType { get; private set; }

        /// <summary>
        /// The function that selects the key for sorting.
        /// </summary>
        internal Expression<Func<TDocument, object>> KeySelector { get; private set; }

        internal GoSortRule(OrderType orderType, Expression<Func<TDocument, object>> keySelector)
        {
            OrderType = orderType;
            KeySelector = keySelector;
        }
    }
}
