using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DDD.Application.MVC.Core.Filters
{
      public class CustomRemoteAttribute : RemoteAttribute
    {
        public CustomRemoteAttribute(string action, string controller, string area)
            : base(action, controller, area)
        {
            this.RouteData["area"] = area;//就是为了让空area时，可以访问根路由
        }
    }
}
