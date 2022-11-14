using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    public class GoProjectionDefinition<TDocument>
    {
        internal ProjectionDefinition<TDocument> MongoProjectionDefinition { get; set; }

        internal GoProjectionDefinition() { }

        public GoProjectionDefinition<TDocument> Include(Expression<Func<TDocument, object>> field)
        {
            MongoProjectionDefinition = MongoProjectionDefinition.Include(field);
            return this;
        }

        public GoProjectionDefinition<TDocument> Exclude(Expression<Func<TDocument, object>> field)
        {
            MongoProjectionDefinition = MongoProjectionDefinition.Exclude(field);
            return this;
        }
    }
}
