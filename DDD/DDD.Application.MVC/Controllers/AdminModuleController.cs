using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DDD.Application.Model.VO;
using DDD.Application.MVC.Core.Helpers;
using DDD.Application.MVC.Filters;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.Common;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;
using DDD.Utility;

namespace DDD.Application.MVC.Controllers
{
    public class AdminModuleController:BaseAdminControllers
    {
        //管理员功能模块
        private readonly IAdminModuleService _adminModuleService;
        private readonly CurrentAdmin _currentAdmin;
        public AdminModuleController(IAdminModuleService adminModuleService,CurrentAdmin currentAdmin)
        {
            _adminModuleService = adminModuleService;
            _currentAdmin = currentAdmin;
        }

        //模块主页
        [HttpGet]
        public ActionResult Index(int? Fid)
        {
            if (Fid == null)
            {
                AddViewBagF(0);
                IEnumerable<AdminModule> listmodule = _adminModuleService.GetMany(new DirectSpecification<AdminModule>(am => am.FID == 0)).OrderBy(am => am.SortFlag);
                return View(listmodule);
            }
            else
            {
                var srm = new ShowResultModel();
                srm.IsSuccess = true;
                srm.ReDirectUrl = "GetModuleList/" + Fid;
                return Json(srm, JsonRequestBehavior.AllowGet);
            }
        }

        #region 下拉列表
        /// <summary>
        /// 下拉列表绑定数据
        /// </summary>
        /// <param name="selectFID"></param>
        void AddViewBagF(int selectFID)
        {
            IEnumerable<SelectListItem> listitem =
                new[] { new SelectListItem { Value = "0", Text = "顶级菜单", Selected = (selectFID == 0) } }.Concat(
                    _adminModuleService.GetAll().OrderBy(a => a.SortFlag).ToSelectListItems(0, selectFID));
            var test = _adminModuleService.GetAll().OrderBy(a => a.SortFlag).ToList();
            //指定下拉被选中
            ViewBag.FIDList = listitem;
        }
        #endregion

        #region 读取主页数据
        /// <summary>
        /// 读取主页所有数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetModuleList(int? id)
        {
            if (id == null)
                id = 0;
            IEnumerable<AdminModule> listmodule = _adminModuleService.GetMany(new DirectSpecification<AdminModule>(am => am.FID == id)).OrderBy(am => am.SortFlag);
            AddViewBagF((int)id);//下拉列表绑定
            return PartialView("_List", listmodule);
        }
        #endregion

        //添加
        [HttpGet]
        public ActionResult AddModule(int id)
        {
            AddViewBagF(id);//id为FID
            return View();
        }

        //添加模块
        [HttpPost]
        public ActionResult AddModule(AdminModule adminModule)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _adminModuleService.AddModule(adminModule);
                    srm.TipMsg = "新增功能模块成功!<br/><br/><input type='button' class='box-buttonadd' value='继续添加'  onclick='window.document.getElementById(&#34;lhgfrm_divaddModule&#34;).contentWindow.Goon();'/><input type='button' class='box-buttonclose' value='取消' onclick='window.document.getElementById(&#34;lhgfrm_divaddModule&#34;).contentWindow.Undo(" + adminModule.FID + ")'/>";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                    LoggerHelper.Log("【系统后台新增功能模块】出错,系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                     (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }

        //单个删除
        [HttpPost]
        public ActionResult DeleteModule(int id, int fid)
        {
            try
            {
                _adminModuleService.DeleteModule(id);
                IEnumerable<AdminModule> listmodule = _adminModuleService.GetMany(new DirectSpecification<AdminModule>(am => am.FID == fid)).OrderBy(am => am.SortFlag);
                AddViewBagF(fid);//下拉列表
                ViewBag.Alert = "删除成功!";
                return PartialView("_List", listmodule);
            }
            catch (Exception e)
            {
                ViewBag.Alert = e.Message;
                IEnumerable<AdminModule> listmodule = _adminModuleService.GetMany(new DirectSpecification<AdminModule>(am => am.FID == fid)).OrderBy(am => am.SortFlag);
                AddViewBagF(fid);//下拉列表
                return PartialView("_List", listmodule);
            }
        }

        //批量删除
        [HttpPost]
        public ActionResult DeleteModuleList(string ids, int fid)
        {
            ShowResultModel res = new ShowResultModel();
            try
            {
                _adminModuleService.DeleteList(ids);
                res.IsSuccess = true;
                res.TipMsg = "删除成功!";
                res.ReDirectUrl = "GetModuleList/" + fid;
            }
            catch (InvalidOperationException ex)
            {
                res.TipMsg = ex.Message;
            }
            catch (Exception e)
            {
                res.TipMsg = e.Message;
                LoggerHelper.Log("【系统后台功能模块批量删除】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                 (e.InnerException == null ? e.Message : e.InnerException.ToString()));
            }
            return Json(res);
        }

        //更新读取
        [HttpGet]
        public ActionResult EditModule(int id)
        {
            AdminModule adm = _adminModuleService.GetByCondition(new DirectSpecification<AdminModule>(am => am.AMID == id));
            AddViewBagF(adm.FID);//下拉列表
            return View(adm);
        }

        //更新操作
        [HttpPost]
        public ActionResult EditModule(AdminModule adm)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _adminModuleService.EditModule(adm);
                    srm.TipMsg = "修改功能模块成功!";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                    LoggerHelper.Log("【系统后台功能模块修改】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                     (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }

        //上/下 排序  Flag=true 向上移动  Flag =false 向下移动
        [HttpPost]
        public ActionResult Move(int id, bool Flag, int fid)
        {
            try
            {
                _adminModuleService.Move(id, Flag);
                IEnumerable<AdminModule> listmodule = _adminModuleService.GetMany(new DirectSpecification<AdminModule>(am => am.FID == fid)).OrderBy(am => am.SortFlag);
                AddViewBagF(fid);//下拉列表
                return PartialView("_List", listmodule);
            }
            catch (Exception e)
            {
                ViewBag.Alert = e.Message;
                IEnumerable<AdminModule> listmodule = _adminModuleService.GetMany(new DirectSpecification<AdminModule>(am => am.FID == fid)).OrderBy(am => am.SortFlag);
                AddViewBagF(fid);//下拉列表
                return PartialView("_List", listmodule);
            }
        }

        //是/否 禁用
        [HttpPost]
        public ActionResult Enable(string ids, bool isEnable, int fid)
        {
            ShowResultModel srm = new ShowResultModel();
            try
            {
                _adminModuleService.Enable(ids, isEnable);
                srm.IsSuccess = true;
                srm.TipMsg = "操作成功!";
                srm.ReDirectUrl = "GetModuleList/" + fid;
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception e)
            {
                srm.TipMsg = e.Message;
                LoggerHelper.Log("【系统后台功能模块启用/禁用】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                 (e.InnerException == null ? e.Message : e.InnerException.ToString()));
            }
            return Json(srm);
        }
    }
}
