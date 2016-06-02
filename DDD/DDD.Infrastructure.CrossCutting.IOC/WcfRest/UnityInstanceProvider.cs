using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace DDD.Infrastructure.CrossCutting.IOC.WcfRest
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;
        private readonly IUnityContainer _container;

        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            _serviceType = serviceType;
            _container = container;
        }


        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _container.Resolve(_serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }

    }
}
