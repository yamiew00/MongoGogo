using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MongoGogo.Connection.Builders.Updates
{
    /// <summary>
    /// Provides an interface to construct update definitions in a MongoDB-compatible way.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public interface IGoUpdateDefinition<TDocument>
    {
        /// <summary>
        /// Creates a $set update for a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to be set on the field.</param>
        /// <returns>A defined update builder.</returns>
        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value);

        /// <summary>
        /// Creates a $setOnInsert update for a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to be set on the field.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> SetOnInsert<TField>(Expression<Func<TDocument, TField>> field,
                                                                 TField value);

        /// <summary>
        /// Creates an $unset update for a field.
        /// </summary>
        /// <param name="field">An expression that indicates the field to be unset.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Unset(Expression<Func<TDocument, object>> field);

        /// <summary>
        /// Creates an $inc update for a field, incrementing its value by a specified amount.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be incremented.</typeparam>
        /// <param name="field">An expression that indicates the field to be incremented.</param>
        /// <param name="value">The amount by which to increment the field's value.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Inc<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value);

        /// <summary>
        /// Creates a $max update for a field, updating its value if the specified value is greater.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to compare with the current field value.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Max<TField>(Expression<Func<TDocument, TField>> field,
                                                 TField value);

        /// <summary>
        /// Creates a $min update for a field, updating its value if the specified value is smaller.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to compare with the current field value.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Min<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value);

        /// <summary>
        /// Creates a $mul update for a field, multiplying its value by a specified amount.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to multiply the field's value with.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Mul<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value);

        /// <summary>
        /// Creates a $addToSet update for a field, adding a value to an array only if it does not already exist.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="value">The value to add to the array.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> AddToSet<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                             TItem value);

        /// <summary>
        /// Creates a $addToSet update for a field with the $each modifier, adding multiple values to an array only if they do not already exist.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="values">The values to add to the array.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> AddToSetEach<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                                 IEnumerable<TItem> values);

        /// <summary>
        /// Creates a $pop update for a field, removing the first element of an array.
        /// </summary>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> PopFirst(Expression<Func<TDocument, object>> field);

        /// <summary>
        /// Creates a $pop update for a field, removing the last element of an array.
        /// </summary>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> PopLast(Expression<Func<TDocument, object>> field);

        /// <summary>
        /// Creates a $pull update for a field, removing any array elements that match a specified value.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="value">The value to remove from the array.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Pull<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                         TItem value);

        /// <summary>
        /// Creates a $pullAll update for a field, removing any array elements that match any of a specified set of values.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="values">The values to remove from the array.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> PullAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                            IEnumerable<TItem> values);

        /// <summary>
        /// Creates a $pull update for a field with a filter, removing any array elements that match the filter.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="filter">A filter to match the elements to be removed.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> PullFilter<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                               Expression<Func<TItem, bool>> filter);

        /// <summary>
        /// Creates a $push update for a field, appending a value to an array.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="value">The value to append to the array.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Push<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                         TItem value);

        /// <summary>
        /// Creates a $push update for a field with the $each modifier, appending multiple values to an array.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the array.</typeparam>
        /// <param name="field">An expression that indicates the array field.</param>
        /// <param name="values">The values to append to the array.</param>
        /// <param name="slice">Optional. If specified, limits the size of the array.</param>
        /// <param name="position">Optional. If specified, inserts elements at a specified position in the array.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> PushEach<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                             IEnumerable<TItem> values,
                                                             int? slice = null,
                                                             int? position = null);

        /// <summary>
        /// Creates a $currentDate update for a field, setting it to the current date.
        /// </summary>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="type">Optional. The type of the date to be set. If not specified, a timestamp will be used.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> CurrentDate(Expression<Func<TDocument, object>> field,
                                                         UpdateDefinitionCurrentDateType? type = null);

        /// <summary>
        /// Creates a bitwise AND update for a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to use in the bitwise operation.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> BitwiseAnd<TField>(Expression<Func<TDocument, TField>> field,
                                                                TField value);

        /// <summary>
        /// Creates a bitwise OR update for a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to use in the bitwise operation.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> BitwiseOr<TField>(Expression<Func<TDocument, TField>> field,
                                                               TField value);

        /// <summary>
        /// Creates a bitwise XOR update for a field.
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated.</typeparam>
        /// <param name="field">An expression that indicates the field to be updated.</param>
        /// <param name="value">The value to use in the bitwise operation.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> BitwiseXor<TField>(Expression<Func<TDocument, TField>> field,
                                                                TField value);

        /// <summary>
        /// Creates a $rename update for a field, changing the field's name.
        /// </summary>
        /// <param name="field">An expression that indicates the field to be renamed.</param>
        /// <param name="newName">The new name for the field.</param>
        /// <returns>An update definition for the MongoDB driver.</returns>
        public GoUpdateDefinition<TDocument> Rename(Expression<Func<TDocument, object>> field,
                                                    string newName);
    }
}
