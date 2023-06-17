using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Represents an update definition for MongoDB operations.
    /// The definition can be built up using chained method calls.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>

    public class GoUpdateDefinition<TDocument>
    {
        internal UpdateDefinition<TDocument> MongoUpdateDefinition { get; set; }

        internal GoUpdateDefinition(){}

        /// <summary>
        /// Adds a $set update to the current update definition.
        /// The $set operator replaces the value of a field with the specified value.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field selector expression.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The current update definition after the $set update is added.</returns>
        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Set(field, value);
            return this;
        }

        /// <summary>
        /// Adds a $unset update to the current update definition.
        /// The $unset operator removes the specified field from a document.
        /// </summary>
        /// <param name="field">The field selector expression.</param>
        /// <returns>The current update definition after the $unset update is added.</returns>
        public GoUpdateDefinition<TDocument> Unset(Expression<Func<TDocument, object>> field)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Unset(field);
            return this;
        }
    }
}
