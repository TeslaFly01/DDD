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
using DDD.Domain.Specification;
using DDD.Utility;

namespace DDD.Application.MVC.Controllers
{
    //管理员角色
    public class AdminRoleController : BaseAdminControllers
    {
        //管理员角色
        private readonly IAdminRoleService _adminRoleService;
        //管理员功能模块
        private readonly IAdminModuleService _adminModuleService;
        //角色关联
        private readonly IAdminRole_ModuleService _adminRole_ModuleService;
        //action
        private readonly IAdminActionService _adminActionService;
        private readonly CurrentAdmin _currentAdmin;
        public AdminRoleController(IAdminRoleService adminRoleService, IAdminModuleService adminModuleService, IAdminRole_ModuleService adminRole_ModuleService, IAdminActionService adminActionService, CurrentAdmin currentAdmin)
        {
            this._adminRoleService = adminRoleService;
            this._adminModuleService = adminModuleService;
            this._adminRole_ModuleService = adminRole_ModuleService;
            this._adminActionService = adminActionService;
            _currentAdmin = currentAdmin;
        }
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<AdminRole> listrole = _adminRoleService.GetAll();
            return View(listrole);
        }
        //删除  判断管理员是否正在使用当前角色
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ShowResultModel srm = new ShowResultModel();
            try
            {
                _adminRoleService.DeleteRole(id);
                srm.IsSuccess = true;
                srm.TipMsg = "删除成功!";
                srm.ReDirectUrl = "GetRoleList";
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception ex)
            {
                srm.TipMsg = ex.Message;
                LoggerHelper.Log("【管理员角色删除】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
            }
            return Json(srm);
        }
        //读取所有到局部视图
        [HttpGet]
        public ActionResult GetRoleList()
        {
            //读取数据
            IEnumerable<AdminRole> listrole = _adminRoleService.GetAll();
            return PartialView("_List", listrole);
        }
        //添加跳转
        [HttpGet]
        public ActionResult Add()
        {
            //读取功能表数据
            ViewBag.Modules = _adminModuleService.GetAll().OrderBy(c => c.SortFlag).ToList();
            return View();
        }
        //添加功能
        [HttpPost]
        public ActionResult Add(AdminRole arl, string ModuleIds)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            var srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _adminRoleService.AddRole(arl, ModuleIds);
                    srm.TipMsg = "新增角色成功!<br/><br/><input type='button' class='box-buttonadd' value='继续添加'  onclick='window.document.getElementById(&#34;lhgfrm_divaddRole&#34;).contentWindow.Goon();'/><input type='button' class='box-buttonclose' value='取消' onclick='window.document.getElementById(&#34;lhgfrm_divaddRole&#34;).contentWindow.Undo()'/>";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                    LoggerHelper.Log("【管理员角色添加】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }
        //修改跳转
        [HttpGet]
        public ActionResult Edit(int id)//arid角色id
        {
            AdminRole role = _adminRoleService.GetByCondition(new DirectSpecification<AdminRole>(ar => ar.ARID == id));
            IEnumerable<AdminRole_Module> listarm = _adminRole_ModuleService.GetMany(new DirectSpecification<AdminRole_Module>(arm => arm.ARID == id));
            var modules = _adminModuleService.GetAll().OrderBy(c => c.SortFlag).ToList();
            ViewBag.Modules = modules;
            var ids = listarm.Select(x => x.AMID);
            ViewBag.ModulesCheck = modules.Where(x => ids.Contains(x.AMID)).ToList();
            return View(role);
        }
        //修改功能
        [HttpPost]
        public ActionResult Edit(AdminRole arl, string ModuleIds)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            var srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _adminRoleService.UpdateRole(arl, ModuleIds);
                    srm.TipMsg = "修改角色成功!";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                    LoggerHelper.Log("【修改管理员角色】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }
        //设置读取
        [HttpGet]
        public ActionResult Setting(int id)//arid
        {
            var mlist = _adminRoleService.GetRoleModules(id).Where(x => x.FID == 0).OrderBy(x => x.SortFlag).ToList();
            ViewBag.ARID = id;
            return View(mlist);
        }
        //得到二级菜单
        [HttpGet]
        public ActionResult GetSecondModule(int firstid, int arid)
        {
            ShowResultModel srm = new ShowResultModel();
            try
            {
                srm.TipMsg = _adminRoleService.GetCurrRoleSecondModule(arid, firstid);
                if (srm.TipMsg.Length == 0)
                    srm.TipMsg = "无数据";
                srm.IsSuccess = true;
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception ex)
            {
                srm.TipMsg = ex.Message;
                LoggerHelper.Log("【获取二级菜单(角色)】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
            }
            return Json(srm, JsonRequestBehavior.AllowGet);
        }
        //得到action
        [HttpGet]
        public ActionResult GetActions(int secondid, int arid)
        {
            ShowResultModel srm = new ShowResultModel();
            try
            {
                //判断多选框选中还是没有  需要得到角色当前功能的权重值 wgt
                IEnumerable<AdminAction> listAA = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == secondid));

                srm.TipMsg = _adminRoleService.GetActionCheck(listAA, arid);
                if (srm.TipMsg.Length == 0)
                    srm.TipMsg = "无数据";
                srm.IsSuccess = true;
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception ex)
            {
                srm.TipMsg = ex.Message;
                LoggerHelper.Log("【获取action(角色)】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
            }
            return Json(srm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSecondModuleAndActions(int fid, int arid)
        {
            var list = _adminRoleService.GetRoleModules(arid);
            ViewBag.FID = fid;
            return View("_SettingModules", list);
        }

        //设置功能
        [HttpPost]
        public ActionResult Setting(int arid, string CMIDWeight)
        {
            var srm = new ShowResultModel();
            try
            {
                //计算/修改权值
                _adminRoleService.UpdateWeight(arid, CMIDWeight);
                srm.TipMsg = "权限设置成功!";
                srm.IsSuccess = true;
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception ex)
            {
                srm.TipMsg = ex.Message;
                LoggerHelper.Log("【设置权值】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
            }
            return Json(srm);
        }

        public ActionResult Modules(int id)
        {
            var adminRole = _adminRoleService.GetByCondition(new DirectSpecification<AdminRole>(x => x.ARID == id), true);
            var adminModules = adminRole.AdminRole_Modules.Select(x => x.adminModule);
            return View(adminModules);
        }
    }
}
