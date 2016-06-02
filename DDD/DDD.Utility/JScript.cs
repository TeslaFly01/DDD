using System;
using System.Web;
using System.Web.UI;

namespace DDD.Utility
{

    /// <summary>
    /// 一些常用的Js调用(Response.Write 会破坏w3c标准的css样式)
    /// </summary>
    public class JScript
    {

        /// <summary>
        /// 弹出JavaScript小窗口
        /// </summary>
        /// <param name="js">窗口信息</param>
        public static void Alert(string message)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    alert('" + message + "');</Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并刷新当前页
        /// </summary>
        /// <param name="js">窗口信息</param>
        public static void AlertAndRefresh(string message)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    alert('" + message + "');window.location = window.location;</Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并关闭当前页面
        /// </summary>
        /// <param name="js">窗口信息</param>
        public static void AlertAndClose(string message)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    alert('" + message + "');window.opener=null;top.close();</Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并返回一字符串值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rValue"></param>
        public static void AlertAndRValue(string message, string rValue)
        {
            #region
            string js = "<script language=javascript>alert('{0}');window.returnValue={1};</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, rValue));
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并返回一字符串值并关闭子窗口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rValue"></param>
        public static void AlertAndRValueClose(string message, string rValue)
        {
            #region
            string js = "<script language=javascript>alert('{0}');window.returnValue='{1}';window.opener=null;top.close();</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, rValue));
            #endregion
        }

        /// <summary>
        /// 弹出消息框并且转向到新的URL
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="toURL">连接地址</param>
        public static void AlertAndRedirect(string message, string toURL)
        {
            #region
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
            #endregion
        }


        /// <summary>
        /// 弹出JavaScript确认小窗口并返回一字符串值
        /// </summary>
        /// <param name="message">是：继续操作；否：关闭窗口</param>
        /// <param name="rValue"></param>
        public static void ConfirmAndRValue(string message, string rValue)
        {
            #region
            string js = "<script language=javascript>window.returnValue={0};if(!confirm('{1}')){{window.opener=null;top.close();}}</script>";
            HttpContext.Current.Response.Write(string.Format(js,rValue, message));
            //string js = @"<script language=javascript>window.returnValue="+rValue+";if(!confirm('"+message+"')){window.opener=null;top.close();}</script>";
            //HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认是否关闭小窗口并返回一字符串值
        /// </summary>
        /// <param name="message">是：关闭窗口；否：继续操作</param>
        /// <param name="rValue"></param>
        public static void ConfirmCloseAndRValue(string message, string rValue)
        {
            #region
            string js = "<script language=javascript>window.returnValue={0};if(confirm('{1}')){{window.opener=null;top.close();}}</script>";
            HttpContext.Current.Response.Write(string.Format(js, rValue, message));
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认小窗口并返回一字符串值，是－同时刷新当前页面
        /// </summary>
        /// <param name="message">是：继续操作；否：关闭窗口</param>
        /// <param name="rValue"></param>
        public static void ConfirmAndRValueRefresh(string message, string rValue)
        {
            #region
            string js = "<script language=javascript>window.returnValue={0};if(!confirm('{1}')){{window.opener=null;top.close();}}else{{window.location=window.location;}}</script>";
            HttpContext.Current.Response.Write(string.Format(js, rValue, message));
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认是否关闭小窗口并返回一字符串值，否－同时刷新当前页面
        /// </summary>
        /// <param name="message">是：关闭窗口；否：继续操作</param>
        /// <param name="rValue"></param>
        public static void ConfirmCloseAndRValueRefresh(string message, string rValue)
        {
            #region
            string js = "<script language=javascript>window.returnValue={0};if(confirm('{1}')){{window.opener=null;top.close();}}else{{window.location=window.location;}}</script>";
            HttpContext.Current.Response.Write(string.Format(js, rValue, message));
            #endregion
        }

        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public static void GoHistory(int value)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    history.go({0});  
                  </Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
            #endregion
        }

        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public static void CloseWindow()
        {
            #region
            string js = @"<Script language='JavaScript'>
                    window.opener=null;top.close();  
                  </Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
            #endregion
        }

        /// <summary>
        /// 刷新当前页面
        /// </summary>
        public static void Refresh()
        {
            #region
            string js = @"<Script language='JavaScript'>
                    window.location = window.location  
                  </Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        public static void RefreshOpener(string url)
        {
            #region
            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }


        /// <summary>
        /// 刷新父窗口
        /// </summary>
        public static void RefreshParent()
        {
            #region
            string js = @"<Script language='JavaScript'>
                    parent.location=parent.location;
                  </Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 弹出窗口刷新父窗口
        /// </summary>
        public static void RefreshParentByOpener()
        {
            #region
            string js = @"<Script language='JavaScript'>
                    parent.opener.document.location=parent.opener.document.location;
                  </Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }
         
        /// <summary>
        /// 弹出窗口指定父窗口页面跳转
        /// </summary>
        public static void ParentJumpByOpener(string newUrl)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    parent.opener.document.location="+newUrl+";</Script>";
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            #region
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            HttpContext.Current.Response.Write(js);
            #endregion
        }


        /// <summary>
        /// 转向Url制定的页面
        /// </summary>
        /// <param name="url">连接地址</param>
        public static void JavaScriptLocationHref(string url)
        {
            #region
            string js = @"<Script language='JavaScript'>
                    window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            HttpContext.Current.Response.Write(js);
            #endregion
        }

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">连接地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            #region
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);
            #endregion
        }

        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }

        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            #region
            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
            #endregion
        }


    }
}
