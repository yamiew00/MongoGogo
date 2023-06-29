using System;

namespace MongoGogo.Exceptions
{
    internal class GoInvalidTypeException<TDocument> : Exception
    {
        private new string Message = $"The type '{typeof(TDocument).GetFriendlyName()}' must be decorated by MongoCollectionAttribute";
    }
}
