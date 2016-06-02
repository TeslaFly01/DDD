using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using DDD.Infrastructure.CrossCutting.IOC;
using DDD.Infrastructure.CrossCutting.IOC.WcfRest;
using DDD.WCF.WcfRestService.Common;

namespace DDD.WCF.WcfRestService.Common
{
    /// <summary>
    /// 引入简单身份验证的HostFactory
    /// </summary>
    public class PrivateWebServiceHostFactory : BaseServiceHostFactory
    {
        //public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        //{
        //    var host = base.CreateServiceHost(constructorString, baseAddresses);
        //    host.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager();
        //    return host;
        //}

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = base.CreateServiceHost(serviceType, baseAddresses);
            host.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager(); //加入简单身份验证
            return host;          
        }
    }
}