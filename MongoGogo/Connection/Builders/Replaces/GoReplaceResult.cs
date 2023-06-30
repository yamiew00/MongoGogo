using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoGogo.Connection
{
    public class GoReplaceResult
    {
        public long ModifiedCount { get; private set; }

        public BsonValue UpsertedId { get; private set; }

        public GoReplaceResult(ReplaceOneResult mongoUpdateResult)
        {
            ModifiedCount = mongoUpdateResult.ModifiedCount;
            UpsertedId = mongoUpdateResult.UpsertedId;
        }
    }
}
