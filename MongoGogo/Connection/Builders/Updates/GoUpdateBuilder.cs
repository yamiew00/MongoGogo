using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    public class GoUpdateBuilder<TDocument>
    {
        internal GoUpdateBuilder() {}

        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            var goDefinition = new GoUpdateDefinition<TDocument>();
            goDefinition.MongoUpdateDefinition = Builders<TDocument>.Update.Set(field, value);
            return goDefinition;
        }
    }
}
