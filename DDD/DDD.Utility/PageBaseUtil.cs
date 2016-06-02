using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DDD.Utility
{
    public class PageBaseUtil
    {
        public static string WebAppPath() { return HttpContext.Current.Request.ApplicationPath.Length == 1 ? HttpContext.Current.Request.ApplicationPath : HttpContext.Current.Request.ApplicationPath + "/"; }
        public  static string CurrentPageName()
        {
            return HttpContext.Current.Request.Url.Segments[HttpContext.Current.Request.Url.Segments.Length - 1];
        }
    }
}
