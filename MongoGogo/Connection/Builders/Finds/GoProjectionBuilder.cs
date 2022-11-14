using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    public class GoProjectionBuilder<TDocument>
    {
        internal GoProjectionBuilder() {}

        public GoProjectionDefinition<TDocument> Include(Expression<Func<TDocument, object>> field)
        {
            var goProjectionDefinition = new GoProjectionDefinition<TDocument>();
            goProjectionDefinition.MongoProjectionDefinition = Builders<TDocument>.Projection.Include(field);
            return goProjectionDefinition;
        }

        public GoProjectionDefinition<TDocument> Exclude(Expression<Func<TDocument, object>> field)
        {
            var goProjectionDefinition = new GoProjectionDefinition<TDocument>();
            goProjectionDefinition.MongoProjectionDefinition = Builders<TDocument>.Projection.Exclude(field);
            return goProjectionDefinition;
        }
    }
}
