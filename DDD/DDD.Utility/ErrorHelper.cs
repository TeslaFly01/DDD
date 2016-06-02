using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DDD.Utility
{
    public class ErrorHelper
    {
        public static void CommonShowError(string msg)
        {
            if (string.IsNullOrEmpty(msg)) msg = "请不要提交错误参数！";
           HttpContext.Current.Response.Write(msg);
           HttpContext.Current.Response.End();
        }

        public static void CommonShowError_lhg(string msg)
        {
           // HttpContext.Current.Response.Write("<script type=\"text/javascript\">var P = window.parent, D = P.loadinndlg();</script>");
            CommonShowError(msg);
        }
    }
}
