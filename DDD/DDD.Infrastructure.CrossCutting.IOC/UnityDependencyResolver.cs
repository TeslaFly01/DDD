using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace DDD.Infrastructure.CrossCutting.IOC
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        IUnityContainer container;
        public UnityDependencyResolver(IUnityContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            // if (!container.IsRegistered(serviceType)) { return null; }
            try
            {
                return container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            // if (!container.IsRegistered(serviceType)) { return new List<object>(); }
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
