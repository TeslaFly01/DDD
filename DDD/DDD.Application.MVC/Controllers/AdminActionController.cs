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
    //管理员功能action
    public class AdminActionController : BaseAdminControllers
    {
        //操作权限
        private readonly IAdminActionService _adminActionService;
        private readonly CurrentAdmin _currentAdmin;
        public AdminActionController(IAdminActionService adminActionService, CurrentAdmin currentAdmin)
        {
            this._adminActionService = adminActionService;
            _currentAdmin = currentAdmin;
        }

        //主页
        [HttpGet]
        public ActionResult Index(int id)
        {
            //此id为AMID
            IEnumerable<AdminAction> listaction = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == id)).OrderBy(aat => aat.SortFlag);
            ViewBag.Amid = id;
            return View(listaction);
        }

        #region 读取主页数据
        //读取主页所有数据
        [HttpGet]
        public ActionResult GetActionList(int? id)
        {
            if (id == null)//amid
                id = 0;
            IEnumerable<AdminAction> listaction = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == id)).OrderBy(aat => aat.SortFlag);
            ViewBag.Amid = id;
            return PartialView("ActionList", listaction);
        }
        #endregion

        //添加跳转
        [HttpGet]
        public ActionResult AddAction(int id)
        {
            AdminAction aat = new AdminAction() { AMID = id, Weight = null };
            return View(aat);
        }
        //添加功能
        [HttpPost]
        public ActionResult AddAction(AdminAction aat)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _adminActionService.AddAction(aat);
                    srm.TipMsg = "新增操作权限块成功!<br/><br/><input type='button' class='box-buttonadd' value='继续添加'  onclick='window.document.getElementById(&#34;lhgfrm_divaddAction&#34;).contentWindow.Goon();'/><input type='button' class='box-buttonclose' value='取消' onclick='window.document.getElementById(&#34;lhgfrm_divaddAction&#34;).contentWindow.Undo(" + aat.AMID + ");'/>";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                    LoggerHelper.Log("【系统管理员操作权限添加】出错,系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                     (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
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
        public ActionResult EditAction(int id)
        {
            AdminAction aat = _adminActionService.GetByCondition(new DirectSpecification<AdminAction>(aa => aa.AAID == id));
            return View(aat);
        }
        //修改功能
        [HttpPost]
        public ActionResult EditAction(AdminAction aat)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    _adminActionService.EditAction(aat);
                    srm.TipMsg = "修改操作权限成功!";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Log("【系统管理员操作权限修改】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                     (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
                    srm.TipMsg = ex.Message;
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
        public ActionResult DeleteAction(int id, int amid)
        {
            try
            {
                _adminActionService.RemoveAction(id);
                IEnumerable<AdminAction> listaction = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == amid)).OrderBy(aa => aa.SortFlag);
                ViewBag.Amid = amid;
                return PartialView("ActionList", listaction);
            }
            catch (Exception e)
            {
                ViewBag.Alert = e.Message;
                IEnumerable<AdminAction> listaction = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == amid)).OrderBy(aa => aa.SortFlag);
                ViewBag.Amid = amid;
                return PartialView("ActionList", listaction);
            }
        }
        //批量删除
        [HttpPost]
        public ActionResult DeleteActionList(string ids, int amid)
        {
            ShowResultModel res = new ShowResultModel();
            try
            {
                _adminActionService.DeleteList(ids);
                res.IsSuccess = true;
                res.TipMsg ="删除成功!";
                ViewBag.Amid = amid;
                res.ReDirectUrl = "/systemadmin/adminaction/GetActionList/" + amid;
            }
            catch (InvalidOperationException ex)
            {
                res.TipMsg = ex.Message;
            }
            catch (Exception e)
            {
                res.TipMsg = e.Message;
                LoggerHelper.Log("【系统管理员操作权限批量删除】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" +
                                 (e.InnerException == null ? e.Message : e.InnerException.ToString()));
            }
            return Json(res);
        }
        //排序
        [HttpPost]
        public ActionResult Move(int id, bool Flag, int amid)
        {
            try
            {
                _adminActionService.Move(id, Flag);
                IEnumerable<AdminAction> listaction = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == amid)).OrderBy(aa => aa.SortFlag);
                ViewBag.Amid = amid;
                return PartialView("ActionList", listaction);
            }
            catch (Exception e)
            {
                ViewBag.Alert = e.Message;
                ViewBag.Amid = amid;
                IEnumerable<AdminAction> listaction = _adminActionService.GetMany(new DirectSpecification<AdminAction>(aa => aa.AMID == amid)).OrderBy(aa => aa.SortFlag);
                return PartialView("ActionList", listaction);
            }
        }
    }
}
