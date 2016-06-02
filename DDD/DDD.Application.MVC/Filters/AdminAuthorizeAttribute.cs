using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DDD.Application.Model.VO;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.Common;
using DDD.Domain.Model.Entities.Admin;
using DDD.Infrastructure.CrossCutting.IOC;
using Microsoft.Practices.Unity;

namespace DDD.Application.MVC.Filters
{
    /// <summary>
    /// 系统管理员权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        [Dependency]
        public IAdminModuleService adminModuleService { get; set; }
        [Dependency]
        public ISystemAdminService systemAdminService { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ExcAdminAuthAttribute), true);
            var isExc = attrs.Length == 1;//是否取消验证
            if (isExc) return;

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            string myRole = string.Empty;
            Uri url = filterContext.HttpContext.Request.Url;
            string fileurl = "/" + url.Segments[url.Segments.Length - 2] + url.Segments[url.Segments.Length - 1];
            AdminModule AMentity = null;//根据url取得功能模块的Forms角色名称
            try
            {
                AMentity = adminModuleService.GetOneByPageUrl(fileurl);
            }
            catch
            {
            }
            myRole = AMentity == null ? string.Empty : AMentity.FormRoleName;
            base.Roles = string.IsNullOrEmpty(base.Roles) ? myRole : base.Roles;//硬编码设置了Roles优先于数据库配置的Forms角色

            //登录验证、Forms角色验证
            base.OnAuthorization(filterContext);
            base.Roles = string.Empty;  //showstopper: 验证过后，避免另一角色的用户登录后身份验证冲突，清除可能存在的Roles缓存
            if (filterContext.Result is HttpUnauthorizedResult) return;

            //只能放在这里创建对象实例，才能实现验证登录成功后调用
            CurrentAdmin curAdmin = UnityContainerFactory.Instance.CurrentContainer.Resolve<CurrentAdmin>();

            //同一帐号不能多人同时登陆使用检测
            try
            {
                if (!systemAdminService.CheckIsRepeatLogon(curAdmin.AdminInfo.SAID, curAdmin.LoginedIP))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                        filterContext.Result = new JsonResult()
                        {
                            Data = new ShowResultModel() { TipMsg = "很抱歉，你被迫下线，该帐号已在别处登录！" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    else
                        filterContext.Result = new ContentResult { Content = "很抱歉，你被迫下线，该帐号已在别处登录！" };

                    return;
                }
            }
            catch
            {
            }

            //Notice:要么同一功能菜单下功能模块的Forms角色名称要不同，要么就要增加设置操作权限[查看]的Controller/Action Name，才能避免登录成功的用户，通过url进入相同Forms角色的页面


            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            //filterContext.HttpContext.Response.Write(myRole);
            //  filterContext.Result = new HttpUnauthorizedResult();//直接URL输入的页面地址跳转到登陆页

            //功能权限检测


            //根据Controller与Action取得当前用户拥有关联该功能模块的权值，逻辑并后，是否等于该权值
            try
            {
                if (!curAdmin.CheckActionWeight(controller, action))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                        filterContext.Result = new JsonResult()
                        {
                            Data = new ShowResultModel() { TipMsg = "抱歉,你不具有当前操作的权限！" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    else
                        filterContext.Result = new ContentResult { Content = "抱歉,你不具有当前操作的权限！" };
                }
            }
            catch
            {
            }
            //filterContext.RequestContext.HttpContext.Response.Write("无权访问");
            //filterContext.RequestContext.HttpContext.Response.End();  
        }
    }
}
