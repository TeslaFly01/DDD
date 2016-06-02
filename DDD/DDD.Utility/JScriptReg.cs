using System;
using System.Web;
using System.Web.UI;

namespace DDD.Utility
{
    /// <summary>
    /// 用注册脚本类输出js (避免破坏w3c标准的css)
    /// </summary>
    public class JScriptReg
    {
        public JScriptReg(Page sender)
        {
            jsSender = sender;
        }
        Page jsSender;
        ClientScriptManager cs;
        /// <summary>
        /// 弹出JavaScript小窗口
        /// </summary>
        /// <param name="js">窗口信息</param>
        public void Alert(string message)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    alert('" + message + "');</Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(),"AlertJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertJS", js, false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并刷新当前页
        /// </summary>
        /// <param name="js">窗口信息</param>
        public void AlertAndRefresh(string message)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    alert('" + message + "');window.location = window.location;</Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndRefreshJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndRefreshJS", js, false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并关闭当前页面
        /// </summary>
        /// <param name="js">窗口信息</param>
        public void AlertAndClose(string message)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    alert('" + message + "');window.opener=null;top.close();</Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndCloseJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndCloseJS", js, false);
            }
            #endregion
        }

        public void AlertAndClose_lhg(string message)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>alert('{0}');dg.cancel();</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndClose_lhgJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndClose_lhgJS", string.Format(js, message), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并返回一字符串值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rValue"></param>
        public void AlertAndRValue(string message, string rValue)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>alert('{0}');window.returnValue={1};</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndRValueJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndRValueJS", string.Format(js, message, rValue), false);
            }
            #endregion
        }

        //调用本页的RefreshP()
        public void AlertAndRefreshP_lhg(string message)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>alert('{0}');RefreshP();</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndRefreshP_lhgJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndRefreshP_lhgJS", string.Format(js, message), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript小窗口并返回一字符串值并关闭子窗口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rValue"></param>
        public void AlertAndRValueClose(string message, string rValue)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>alert('{0}');window.returnValue='{1}';window.opener=null;top.close();</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndRValueCloseJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndRValueCloseJS", string.Format(js, message, rValue), false);
            }
            #endregion
        }

        public void AlertAndRefreshPClose_lhg(string message, string CallBackFn)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>alert('{0}');{1}();dg.cancel();</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndRefreshPClose_lhgJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndRefreshPClose_lhgJS", string.Format(js, message, CallBackFn), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出消息框并且转向到新的URL
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="toURL">连接地址</param>
        public void AlertAndRedirect(string message, string toURL)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "AlertAndRedirectJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "AlertAndRedirectJS", string.Format(js, message, toURL), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认小窗口并返回一字符串值
        /// </summary>
        /// <param name="message">是：继续操作；否：关闭窗口</param>
        /// <param name="rValue"></param>
        public void ConfirmAndRValue(string message, string rValue)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>window.returnValue={0};if(!confirm('{1}')){{window.opener=null;top.close();}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ConfirmAndRValueJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ConfirmAndRValueJS", string.Format(js, rValue, message), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认是否关闭小窗口并返回一字符串值
        /// </summary>
        /// <param name="message">是：关闭窗口；否：继续操作</param>
        /// <param name="rValue"></param>
        public void ConfirmCloseAndRValue(string message, string rValue)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>window.returnValue={0};if(confirm('{1}')){{window.opener=null;top.close();}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ConfirmCloseAndRValueJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ConfirmCloseAndRValueJS", string.Format(js, rValue, message), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认小窗口并返回一字符串值，是－同时刷新当前页面
        /// </summary>
        /// <param name="message">是：继续操作；否：关闭窗口</param>
        /// <param name="rValue"></param>
        public void ConfirmAndRValueRefresh(string message, string rValue)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>window.returnValue={0};if(!confirm('{1}')){{window.opener=null;top.close();}}else{{window.location=window.location;}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ConfirmCloseAndRValueJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ConfirmCloseAndRValueJS", string.Format(js, rValue, message), false);
            }
            #endregion
        }

        public void ConfirmAndMePRefresh_lhg(string message, string CallBackFn)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>{1}();if(!confirm('{0}')){{dg.cancel();}}else{{window.location=window.location;}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "lhg_ConfirmAndMePRefreshJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "lhg_ConfirmAndMePRefreshJS", string.Format(js, message,CallBackFn), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出JavaScript确认是否关闭小窗口并返回一字符串值，否－同时刷新当前页面
        /// </summary>
        /// <param name="message">是：关闭窗口；否：继续操作</param>
        /// <param name="rValue"></param>
        public void ConfirmCloseAndRValueRefresh(string message, string rValue)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>window.returnValue={0};if(confirm('{1}')){{window.opener=null;top.close();}}else{{window.location=window.location;}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ConfirmCloseAndRValueRefreshJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ConfirmCloseAndRValueRefreshJS", string.Format(js, rValue, message), false);
            }
            #endregion
        }

        public void ConfirmCloseAndPRefresh_lhg(string message, string CallBackFn)
        {
            #region
            cs = jsSender.ClientScript;
            string js = "<script language=javascript>{1}();if(confirm('{0}')){{dg.cancel();}}else{{window.location=window.location;}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "lhg_ConfirmCloseAndPRefreshJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "lhg_ConfirmCloseAndPRefreshJS", string.Format(js, message, CallBackFn), false);
            }
            #endregion
        }

        /// <summary>
        /// 弹出确认是否打开新页面
        /// </summary>
        /// <param name="message"></param>
        /// <param name="openUrl"></param>
        /// <param name="IsRefresh">是否刷新本页面</param>
        public void ConfirmOpenAndRefresh(string message, string openUrl,bool IsRefresh)
        {
            #region
            cs = jsSender.ClientScript;
            string js ;
            if(IsRefresh)js = "<script language=javascript>if(confirm('{0}')){{window.open('{1}');}}else{{window.location=window.location;}}</script>";
            else js = "<script language=javascript>if(confirm('{0}')){{window.open('{1}');}}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ConfirmOpenAndRefreshJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ConfirmOpenAndRefreshJS", string.Format(js, message,openUrl), false);
            }
            #endregion
        }

        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">-1/1</param>
        public void GoHistory(int value)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    history.go({0});  
                  </Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "GoHistoryJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "GoHistoryJS", string.Format(js, value), false);
            }
            #endregion
        }

        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public void CloseWindow()
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    window.opener=null;top.close();  
                  </Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "CloseWindowJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "CloseWindowJS", js, false);
            }
            HttpContext.Current.Response.End();
            #endregion
        }

        /// <summary>
        /// 刷新当前页面
        /// </summary>
        public void Refresh()
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    window.location = window.location  
                  </Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "RefreshJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "RefreshJS", js, false);
            }
            #endregion
        }

        /// <summary>
        /// 刷新父窗口
        /// </summary>
        public void RefreshParent()
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    parent.location=parent.location;
                  </Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "RefreshParentJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "RefreshParentJS", js, false);
            }
            #endregion
            
        }

        /// <summary>
        /// 弹出窗口刷新父窗口
        /// </summary>
        public void RefreshParentByOpener()
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    parent.opener.document.location=parent.opener.document.location;
                  </Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "RefreshParentByOpenerJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "RefreshParentByOpenerJS", js, false);
            }
            #endregion

        }


        /// <summary>
        /// 弹出窗口指定父窗口页面跳转
        /// </summary>
        public void ParentJumpByOpener(string newUrl)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    parent.opener.document.location='"+newUrl+"';</Script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ParentJumpByOpenerJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ParentJumpByOpenerJS", js, false);
            }
            #endregion

        }


        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        public void RefreshOpener(string url)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<script>try{top.location=""" + url + @"""}catch(e){location=""" + url + @"""}</script>";
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "RefreshOpenerJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "RefreshOpenerJS", js, false);
            }
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
        public void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";

            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "OpenWebFormSizeJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "OpenWebFormSizeJS", js, false);
            }
            #endregion
        }


        /// <summary>
        /// 转向Url制定的页面
        /// </summary>
        /// <param name="url">连接地址</param>
        public void JavaScriptLocationHref(string url)
        {
            #region
            cs = jsSender.ClientScript;
            string js = @"<Script language='JavaScript'>
                    window.location.replace('{0}');
                  </Script>";
            js = string.Format(js, url);
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "LocationHrefJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "LocationHrefJS", js, false);
            }
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
        public void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
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

        public  void ShowModalDialogWindow(string webFormUrl, string features)
        {
            cs = jsSender.ClientScript;
            string js = ShowModalDialogJavascript(webFormUrl, features);
            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "ShowModalDialogWindowJS"))
            {
                cs.RegisterClientScriptBlock(this.GetType(), "ShowModalDialogWindowJS", js, false);
            }
        }

        public string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            #region
            string js = @"<script language=javascript>							
							showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
            #endregion
        }


    }
}
