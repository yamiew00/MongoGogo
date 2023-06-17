using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoGogo.Connection
{
    /// <summary>
    ///  A MongoDB document in the database.
    /// </summary>
    public class GoDocument
    {
        /// <summary>
        /// The _id field necessary for every document.
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        [BsonIgnoreIfDefault]

        public ObjectId _id { get; set; }
    }
}
