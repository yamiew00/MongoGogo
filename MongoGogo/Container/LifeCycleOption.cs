namespace MongoGogo
{
    public class LifeCycleOption
    {
        /// <summary>
        /// Lifecycle of context in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType ContextLifeCycle { get; set; } = LifeCycleType.Scoped;

        /// <summary>
        /// Lifecycle of database in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType DatabaseLifeCycle { get; set; } = LifeCycleType.Scoped;

        /// <summary>
        /// Lifecycle of collection in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType CollectionLifeCycle { get; set; } = LifeCycleType.Scoped;

        /// <summary>
        /// Lifecycle of collection in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType RepositoryLifeCycle { get; set; } = LifeCycleType.Scoped;
    }

    public enum LifeCycleType
    {
        Transient,
        Scoped,
        Singleton
    }
}
