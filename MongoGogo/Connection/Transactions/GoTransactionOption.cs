namespace MongoGogo.Connection.Transactions
{
    public class GoTransactionOption
    {
        /// <summary>
        /// When true or unspecified, an application will read its own writes and subsequent
        /// reads will never observe an earlier version of the data.
        /// </summary>
        public bool? CausalConsistency { get; set; } = true;
    }
}
