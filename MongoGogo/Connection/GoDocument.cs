using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoGogo.Connection
{
    public class GoDocument
    {
        /// <summary>
        ///  A MongoDB document in the database.
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        [BsonIgnoreIfDefault]

        public ObjectId _id { get; set; }
    }
}
