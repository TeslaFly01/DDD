using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;

namespace DDD.Application.MVC.Core.Controllers.Validate
{
    public class SysAdminValiController : BaseControllers
    {
        private readonly SystemAdminService _systemAdminService;
        public SysAdminValiController(SystemAdminService systemAdminService)
        {
            this._systemAdminService = systemAdminService;
        }

        /// <summary>
        /// 管理员用户名有效性验证（唯一性、屏蔽关键字用户名等等）
        /// </summary>
        /// <param name="SAName"></param>
        /// <returns></returns>
        public ActionResult CheckAdminName(string SAName)
        {
            bool chkFlag = false;
            //应该调用领域服务层的判断方法
            if (_systemAdminService.Exists(new DirectSpecification<SystemAdmin>(x => x.SAName == SAName)))
            {
                return Json("该管理员帐号已存在！", JsonRequestBehavior.AllowGet);
            }
            else
            {
                chkFlag = true;
            }
            return Json(chkFlag, JsonRequestBehavior.AllowGet);
        }
    }
}
