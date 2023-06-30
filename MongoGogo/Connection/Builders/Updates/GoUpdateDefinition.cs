using MongoDB.Driver;
using MongoGogo.Connection.Builders.Updates;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Represents an update definition for MongoDB operations.
    /// The definition can be built up using chained method calls.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>

    public class GoUpdateDefinition<TDocument> : IGoUpdateDefinition<TDocument>
    {
        internal UpdateDefinition<TDocument> MongoUpdateDefinition { get; private set; }

        internal GoUpdateDefinition(UpdateDefinition<TDocument> initialUpdateDefinition) 
        {
            MongoUpdateDefinition = initialUpdateDefinition;
        }

        /// <summary>
        /// Adds a $set update to the current update definition.
        /// The $set operator replaces the value of a field with the specified value.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field selector expression.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The current update definition after the $set update is added.</returns>
        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
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

        public GoUpdateDefinition<TDocument> SetOnInsert<TField>(Expression<Func<TDocument, TField>> field,
                                                                 TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.SetOnInsert(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> Inc<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Inc(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> Max<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Max(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> Min<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Min(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> Mul<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Mul(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> AddToSet<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                             TItem value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.AddToSet(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> AddToSetEach<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                                 IEnumerable<TItem> values)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.AddToSetEach(field, values);
            return this;
        }

        public GoUpdateDefinition<TDocument> PopFirst(Expression<Func<TDocument, object>> field)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.PopFirst(field);
            return this;
        }

        public GoUpdateDefinition<TDocument> PopLast(Expression<Func<TDocument, object>> field)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.PopLast(field);
            return this;
        }

        public GoUpdateDefinition<TDocument> Pull<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                         TItem value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Pull(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> PullAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                            IEnumerable<TItem> values)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.PullAll(field, values);
            return this;
        }

        public GoUpdateDefinition<TDocument> PullFilter<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                               Expression<Func<TItem, bool>> filter)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.PullFilter(field, filter);
            return this;
        }

        public GoUpdateDefinition<TDocument> Push<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                         TItem value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Push(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> PushEach<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                             IEnumerable<TItem> values,
                                                             int? slice = null,
                                                             int? position = null)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.PushEach(field, values, slice, position);
            return this;
        }

        public GoUpdateDefinition<TDocument> CurrentDate(Expression<Func<TDocument, object>> field,
                                                         UpdateDefinitionCurrentDateType? type = null)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.CurrentDate(field, type);
            return this;
        }

        public GoUpdateDefinition<TDocument> BitwiseAnd<TField>(Expression<Func<TDocument, TField>> field,
                                                                TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.BitwiseAnd(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> BitwiseOr<TField>(Expression<Func<TDocument, TField>> field,
                                                               TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.BitwiseOr(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> BitwiseXor<TField>(Expression<Func<TDocument, TField>> field,
                                                                TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.BitwiseXor(field, value);
            return this;
        }

        public GoUpdateDefinition<TDocument> Rename(Expression<Func<TDocument, object>> field,
                                                    string newName)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Rename(field, newName);
            return this;
        }
    }
}
