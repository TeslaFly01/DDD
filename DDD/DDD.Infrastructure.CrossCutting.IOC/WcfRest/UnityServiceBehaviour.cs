using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace DDD.Infrastructure.CrossCutting.IOC.WcfRest
{
    public class UnityServiceBehaviour : IServiceBehavior
    {
        private readonly IUnityContainer _container;

        public UnityServiceBehaviour(IUnityContainer container)
        {
            _container = container;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var endpointDispatcher in
                serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany(
                    channelDispatcher => channelDispatcher.Endpoints))
            {
                endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(_container, serviceDescription.ServiceType);
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
