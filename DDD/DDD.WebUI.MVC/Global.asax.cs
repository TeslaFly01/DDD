using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using DDD.Application.MVC.Core.Filters;
using DDD.Infrastructure.CrossCutting.IOC;
using DDD.Utility;
using Microsoft.Practices.Unity;

namespace DDD.WebUI.MVC
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }

        //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    // filters.Add(new HandleErrorAttribute());
        //    filters.Add(new HandleErrorWithLog4netAttribute());
        //}

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BundleTable.EnableOptimizations = true;

            //注册unity
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);
            IUnityContainer container = UnityContainerFactory.Instance.CurrentContainer;
            var provider = new UnityFilterAttributeFilterProvider(container);
            FilterProviders.Providers.Add(provider);//为Filter加入支持Unity的Provider
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            var id = Context.User.Identity as FormsIdentity;
            if (id != null && id.IsAuthenticated)
            {
                string[] userInfo = id.Ticket.UserData.Split(';');
                string[] roles = userInfo[0].Split(',');
                Context.User = new GenericPrincipal(id, roles);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // 發生未處理錯誤時執行的程式碼
            Exception objErr = Server.GetLastError();
            if (HttpContext.Current.User.Identity.IsAuthenticated == false || objErr is AuthenticationException)
                Server.TransferRequest("/");
            else
            {
                Response.Write("网络错误，请稍后再试！");
                LoggerHelper.Log("系统异常，错误原因：" + (objErr.InnerException == null ? objErr.Message : objErr.InnerException.ToString()));
            }
        }
    }
}