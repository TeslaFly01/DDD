using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.CacheService;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Application.Service.Common
{
    /// <summary>
    /// 当前登录系统管理员
    /// </summary>
    public class CurrentAdmin
    {
        // [Dependency]
        // public ISystemAdminService systemAdminService { get; set; }
        readonly ISystemAdminService _systemAdminService;
        readonly IAdminActionService _adminActionService;
        readonly AdminCacheService _adminCacheService;

        public CurrentAdmin(ISystemAdminService systemAdminService, IAdminActionService adminActionService, AdminCacheService adminCacheService)
        {
            this._systemAdminService = systemAdminService;
            this._adminActionService = adminActionService;
            this._adminCacheService = adminCacheService;

            if (!HttpContext.Current.User.Identity.IsAuthenticated) //WCF use OperationContext.Current 
                throw new AuthenticationException("账户未登录");//必须要用户Form验证后才能使用CurrentAdmin
            FormsAuthenticationTicket ticket = (HttpContext.Current.User.Identity as FormsIdentity).Ticket;
            string[] userInfo = ticket.UserData.Split(';');

            //从缓存中取得当前登录用户信息，缓存无，则从数据库取得
            string SAName = ticket.Name;
            AdminInfo = _adminCacheService.GetCache<SystemAdmin>(AdminCacheService.SysAdmin_Current_prefix + SAName);
            if (AdminInfo == null)
            {
                AdminInfo = systemAdminService.GetByCondition(new DirectSpecification<SystemAdmin>(x => x.SAName == SAName), true);
                _adminCacheService.Add<SystemAdmin>(AdminCacheService.SysAdmin_Current_prefix + SAName, AdminInfo, TimeSpan.FromHours(2));
            }


            AdminRoles = AdminInfo.AdminRoles;

            LoginedIP = userInfo[1];

        }

        //用户信息
        public SystemAdmin AdminInfo { get; set; }

        public string LoginedIP { get; set; }

        //菜单权限
        public IEnumerable<AdminModule> AdminModules
        {
            get
            {
                //取得当前管理员拥有角色对应的所有不重复的功能模块
                return _systemAdminService.GetsysAdminModule(AdminInfo);
            }
        }

        //功能权限
        //public IEnumerable<AdminAction> AdminActions { get; set; }

        //管理员角色
        public IEnumerable<AdminRole> AdminRoles { get; set; }

        public bool CheckActionWeight(AdminAction adminAction)
        {
            return CheckAction(adminAction);
        }
        /// <summary>
        /// 验证某操作功能是否具有权限
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="ActionName"></param>
        /// <returns></returns>
        public bool CheckActionWeight(string ControllerName, string ActionName)
        {
            bool result = true;
            AdminAction adminActionEntity = _adminActionService.GetByCondition(new DirectSpecification<AdminAction>(a => a.ControllerName == ControllerName && a.ActionName == ActionName), true);
            if (adminActionEntity != null)
                result = CheckAction(adminActionEntity);
            return result;
        }
        private bool CheckAction(AdminAction adminAction)
        {
            int weight = 0;
            foreach (var adminRole in AdminRoles)
            {
                weight = weight | adminRole.AdminRole_Modules.Where(x => x.AMID == adminAction.AMID).Select(x => x.Weights).FirstOrDefault();//某个角色与某个功能模块一一对应
            }
            bool re = (weight & adminAction.Weight) == adminAction.Weight;
            return re;
        }
    }
}
