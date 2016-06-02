using System;
using System.ServiceModel.Web;
using Microsoft.Practices.Unity;

namespace DDD.Infrastructure.CrossCutting.IOC.WcfRest
{
    //参考：http://wiki.asp.net/page.aspx/1631/wcf-rest-service-dependecy-injection-with-unity-and-ef4-codefirst/

    public class UnityWebServiceHost : WebServiceHost
    {
        protected IUnityContainer _container;

        public UnityWebServiceHost()
        {
        }

        public UnityWebServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
        }

        public UnityWebServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new UnityServiceBehaviour(_container));
            base.OnOpening();
        }
    }

}
