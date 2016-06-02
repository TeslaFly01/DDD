using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using DDD.Infrastructure.CrossCutting.IOC;
using DDD.Infrastructure.CrossCutting.IOC.WcfRest;

namespace DDD.WCF.WcfRestService.Common
{
    /// <summary>
    /// BaseServiceHostFactory： unity依赖注入
    /// </summary>
    public class BaseServiceHostFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var container = UnityContainerFactory.Instance.CurrentContainer;

            return new UnityWebServiceHost(container, serviceType, baseAddresses);
        }
    }

}