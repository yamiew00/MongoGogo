namespace MongoGogo
{
    public class LifeCycleOption
    {
        /// <summary>
        /// Lifecycle of context in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType ContextLifeCycle { get; set; } = LifeCycleType.Singleton;

        /// <summary>
        /// Lifecycle of database in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType DatabaseLifeCycle { get; set; } = LifeCycleType.Singleton;

        /// <summary>
        /// Lifecycle of collection in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType MongoCollectionLifeCycle { get; set; } = LifeCycleType.Singleton;

        /// <summary>
        /// Lifecycle of collection in .net core. Scoped is default.
        /// </summary>
        public LifeCycleType GoCollectionLifeCycle { get; set; } = LifeCycleType.Singleton;
    }

    public enum LifeCycleType
    {
        Transient,
        Scoped,
        Singleton
    }
}
