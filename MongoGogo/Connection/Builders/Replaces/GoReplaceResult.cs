using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoGogo.Connection
{
    public class GoReplaceResult
    {
        public long ModifiedCount { get; private set; }

        public ObjectId? UpsertedId { get; private set; }

        public GoReplaceResult(ReplaceOneResult mongoUpdateResult)
        {
            ModifiedCount = mongoUpdateResult.ModifiedCount;
            UpsertedId = (mongoUpdateResult.UpsertedId == null) ? default : mongoUpdateResult.UpsertedId.AsBsonValue.AsObjectId;
        }
    }
}
