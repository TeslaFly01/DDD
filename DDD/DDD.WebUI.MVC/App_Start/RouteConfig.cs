using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DDD.WebUI.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
             "Core", // Route name
             "Core/{controller}/{action}/{id}", // URL with parameters
             new { id = UrlParameter.Optional } // Parameter defaults
             , new string[] { "DDD.Application.MVC.Core.Controllers" }
             );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // Parameter defaults
                , new string[] { "DDD.Application.MVC.Controllers" }
            );
        }
    }
}