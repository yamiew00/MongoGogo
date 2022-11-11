
namespace MongoGogo.Connection
{
    /// <summary>
    /// Default implementation of GoRepositoryAbstract using DI.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    internal class GoRepository<TDocument> : GoRepositoryAbstract<TDocument>
    {
        public GoRepository(IGoCollection<TDocument> collection) : base(collection)
        {
        }
    }
}
