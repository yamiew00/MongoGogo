using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoGogo.Connection.ObserverItems
{
    internal class GoGenericDocument<TBsonIdType>
    {
        /// <summary>
        /// The _id field necessary for every document.
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        [BsonIgnoreIfDefault]

        internal TBsonIdType _id { get; set; }
    }
}
