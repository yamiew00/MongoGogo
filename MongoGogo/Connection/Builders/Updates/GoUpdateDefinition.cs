using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoGogo.Connection
{
    public class GoUpdateDefinition<TDocument>
    {
        internal UpdateDefinition<TDocument> MongoUpdateDefinition { get; set; }

        internal GoUpdateDefinition(){}

        public GoUpdateDefinition<TDocument> Set<TField>(Expression<Func<TDocument, TField>> field, TField value)
        {
            MongoUpdateDefinition = MongoUpdateDefinition.Set(field, value);
            return this;
        }
    }
}
