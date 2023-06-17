using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Provides the mechanism for building update definitions for MongoDB operations.
    /// This class encapsulates MongoDB.Driver.Builders.Update functionality, providing a more lightweight and user-friendly interface.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>

    public class GoUpdateBuilder<TDocument>
    {
        internal GoUpdateBuilder() {}

        /// <summary>
        /// Creates a new $set update definition for a specified field.
        /// The $set operator replaces the value of a field with the specified value.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field selector expression.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The update definition.</returns>
        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            var goDefinition = new GoUpdateDefinition<TDocument>();
            goDefinition.MongoUpdateDefinition = Builders<TDocument>.Update.Set(field, value);
            return goDefinition;
        }

        /// <summary>
        /// Creates a new $unset update definition for a specified field.
        /// The $unset operator removes the specified field from a document.
        /// </summary>
        /// <param name="field">The field selector expression.</param>
        /// <returns>The update definition.</returns>
        public GoUpdateDefinition<TDocument> Unset(Expression<Func<TDocument, object>> field)
        {
            var goDefinition = new GoUpdateDefinition<TDocument>();
            goDefinition.MongoUpdateDefinition = Builders<TDocument>.Update.Unset(field);
            return goDefinition;
        }
    }
}
