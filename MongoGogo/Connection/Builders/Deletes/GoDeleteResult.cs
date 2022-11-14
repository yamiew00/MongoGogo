using MongoDB.Driver;

namespace MongoGogo.Connection
{
    public class GoDeleteResult
    {
        public long DeletedCount { get; private set; }

        public GoDeleteResult(DeleteResult deleteResult)
        {
            DeletedCount = deleteResult.DeletedCount;
        }
    }
}
