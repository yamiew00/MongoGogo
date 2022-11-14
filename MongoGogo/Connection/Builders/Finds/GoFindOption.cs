namespace MongoGogo.Connection
{
    public class GoFindOption
    {
        public bool AllowDiskUse { get; set; } = false;

        public int? Limit { get; set; } = default;

        public int? Skip { get; set; } = default;
    }
}
