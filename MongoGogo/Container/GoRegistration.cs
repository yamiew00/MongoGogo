using System;

namespace MongoGogo.Container
{
    public class GoRegistration
    {
        public Type RegisteredType { get; private set; }

        public Type MappedType { get; private set; }

        public LifeCycleType LifeTime { get; private set; }

        public object Instance { get; private set; }

        public string RegisteredName { get => RegisteredType.GetFriendlyName(); }

        public string MappedName { get => MappedType.GetFriendlyName(); }

        public GoRegistration(Type registeredType,
                              Type mappedType,
                              LifeCycleType lifeTime)
        {
            RegisteredType = registeredType;
            MappedType = mappedType;
            LifeTime = lifeTime;
            Instance = default;
        }

        public GoRegistration(Type registeredType,
                            object instance,
                            LifeCycleType lifeTime)
        {
            RegisteredType = registeredType;
            LifeTime = lifeTime;
            Instance = instance;
            MappedType = instance.GetType();
        }
    }
}
