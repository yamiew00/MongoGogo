using MongoDB.Driver;
using MongoGogo.Connection.Builders.Updates;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Provides the mechanism for building update definitions for MongoDB operations.
    /// This class encapsulates MongoDB.Driver.Builders.Update functionality, providing a more lightweight and user-friendly interface.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>

    public class GoUpdateBuilder<TDocument> : IGoUpdateDefinition<TDocument>
    {
        internal GoUpdateBuilder() {}

        public GoUpdateDefinition<TDocument> AddToSet<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                             TItem value)
        {
            var updateDefinition = Builders<TDocument>.Update.AddToSet(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> AddToSetEach<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                                 IEnumerable<TItem> values)
        {
            var updateDefinition = Builders<TDocument>.Update.AddToSetEach(field, values);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> BitwiseAnd<TField>(Expression<Func<TDocument, TField>> field,
                                                                TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.BitwiseAnd(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> BitwiseOr<TField>(Expression<Func<TDocument, TField>> field,
                                                               TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.BitwiseOr(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> BitwiseXor<TField>(Expression<Func<TDocument, TField>> field,
                                                                TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.BitwiseXor(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> CurrentDate(Expression<Func<TDocument, object>> field,
                                                         UpdateDefinitionCurrentDateType? type = null)
        {
            var updateDefinition = Builders<TDocument>.Update.CurrentDate(field, type);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Inc<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.Inc(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Max<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.Max(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Min<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.Min(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Mul<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.Mul(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> PopFirst(Expression<Func<TDocument, object>> field)
        {
            var updateDefinition = Builders<TDocument>.Update.PopFirst(field);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> PopLast(Expression<Func<TDocument, object>> field)
        {
            var updateDefinition = Builders<TDocument>.Update.PopLast(field);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Pull<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                         TItem value)
        {
            var updateDefinition = Builders<TDocument>.Update.Pull(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> PullAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                            IEnumerable<TItem> values)
        {
            var updateDefinition = Builders<TDocument>.Update.PullAll(field, values);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> PullFilter<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                               Expression<Func<TItem, bool>> filter)
        {
            var updateDefinition = Builders<TDocument>.Update.PullFilter(field, filter);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Push<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                         TItem value)
        {
            var updateDefinition = Builders<TDocument>.Update.Push(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> PushEach<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> field,
                                                             IEnumerable<TItem> values,
                                                             int? slice = null,
                                                             int? position = null)
        {
            var updateDefinition = Builders<TDocument>.Update.PushEach(field, values, slice, position);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Rename(Expression<Func<TDocument, object>> field,
                                                    string newName)
        {
            var updateDefinition = Builders<TDocument>.Update.Rename(field, newName);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field,
                                                         TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.Set(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> SetOnInsert<TField>(Expression<Func<TDocument, TField>> field,
                                                                 TField value)
        {
            var updateDefinition = Builders<TDocument>.Update.SetOnInsert(field, value);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }

        public GoUpdateDefinition<TDocument> Unset(Expression<Func<TDocument, object>> field)
        {
            var updateDefinition = Builders<TDocument>.Update.Unset(field);
            return new GoUpdateDefinition<TDocument>(updateDefinition);
        }
    }
}
