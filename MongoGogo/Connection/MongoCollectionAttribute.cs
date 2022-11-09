using System;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Represent a MongoDB collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MongoCollectionAttribute : Attribute
    {
        /// <summary>
        /// Type of its database.
        /// </summary>
        public Type DbType { get; private set; }

        /// <summary>
        /// Collection Name. Null value if collection name equals to type name.
        /// </summary>
        public string? GivenName { get; private set; }

        public MongoCollectionAttribute(Type fromDatabase, string? collectionName = default)
        {
            this.DbType = fromDatabase;
            this.GivenName = collectionName;
        }
    }
}
