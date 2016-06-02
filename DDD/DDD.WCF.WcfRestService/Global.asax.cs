using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.ServiceModel;
using DDD.Infrastructure.CrossCutting.IOC;
using DDD.Infrastructure.CrossCutting.IOC.WcfRest;
using DDD.WCF.WcfRestService.Common;
using DDD.WCF.WcfRestService.BusinessService;

namespace DDD.WCF.WcfRestService
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            // Edit the base address of Service1 by replacing the "Service1" string below
            //WebServiceHostFactory factory = new WebServiceHostFactory();
            // RouteTable.Routes.Add(new ServiceRoute("Service1", new WebServiceHostFactory(), typeof(Service1))); 

            //use our custom PrivateWebServiceHostFactory :unity依赖注入、 身份验证 
            
            
            RouteTable.Routes.Add(new ServiceRoute(NotNoticeAPI.NotNoticeRouteName,new PrivateWebServiceHostFactory(), typeof(NotNoticeAPI)));//未提醒
            
            //BaseServiceHostFactory：unity依赖注入
            // RouteTable.Routes.Add(new ServiceRoute(CorpSalesCampaignAPI._CorpSalesCampaignRouteName, new BaseServiceHostFactory(), typeof(CorpSalesCampaignAPI)));
        }


    }
}
