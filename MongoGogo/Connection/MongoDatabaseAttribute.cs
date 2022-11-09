using System;

namespace MongoGogo.Connection
{
    /// <summary>
    /// Represent a MongoDB database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MongoDatabaseAttribute : Attribute
    {
        /// <summary>
        /// Database Name. Null value if database name equals to type name.
        /// </summary>
        public string? GivenName { get; private set; }

        public MongoDatabaseAttribute(string? name = default)
        {
            this.GivenName = name;
        }
    }
}
