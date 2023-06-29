namespace MongoGogo.Connection.Transactions
{
    internal class GoTransactionStatus
    {
        public bool IsSessionStart { get; set; }

        public bool HasAnyOperation { get; set; }
    }
}
