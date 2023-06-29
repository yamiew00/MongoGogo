using System;

namespace MongoGogo.Container
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks> //todo: not a good way to manage serviceProvider</remarks>
    public class GoServiceProvider : IServiceProvider
    {
        private readonly GoContainer _goContainer;

        public GoServiceProvider(GoContainer goContainer)
        {
            this._goContainer = goContainer;
        }

        public object GetService(Type serviceType)
        {
            return _goContainer.Resolve(serviceType);
        }
    }
}
