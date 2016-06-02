using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DDD.Application.Model.VO;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.Common;
using DDD.Domain;
using DDD.Domain.Model.Entities.Admin;
using DDD.Utility;
using Webdiyer.WebControls.Mvc;

namespace DDD.Application.MVC.Controllers
{
    /// <summary>
    /// 管理员操作日志
    /// </summary>
    public class AdminLogController : BaseAdminControllers
    {
        private readonly IAdminLogService _AdminLogService;
        private readonly CurrentAdmin _currentAdmin;
        public AdminLogController(IAdminLogService AdminLogService, CurrentAdmin currentAdmin)
        {
            this._AdminLogService = AdminLogService;
            _currentAdmin = currentAdmin;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List(int? pageIndex, int? pageSize, string OptContent, string UserName, DateTime? FromDate, DateTime? ToDate)
        {
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? 10;

            if (ToDate.HasValue) ToDate = ToDate.Value.AddDays(1);

            PageData<AdminLog> aList = _AdminLogService.Search(pageIndex.Value, pageSize.Value, OptContent, UserName, FromDate, ToDate);
            PagedList<AdminLog> pList = null;
            if (aList.DataList != null)
            {
                pList = new PagedList<AdminLog>(aList.DataList, aList.CurrentPageIndex, pageSize.Value, aList.TotalCount);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_List", pList);
            }
            else return RedirectToAction("Index");

        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            ShowResultModel rs = new ShowResultModel();

            AdminLog entity = _AdminLogService.GetByKey(id);
            if (entity == null)
            {
                rs.TipMsg = "未找到该记录！";
                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            try
            {
                _AdminLogService.Remove(entity);
                rs.TipMsg = "删除成功!";
                rs.IsSuccess = true;
            }
            catch (Exception exc)
            {
                rs.TipMsg = exc.Message;
                LoggerHelper.Log("【删除系统管理员操作日志】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (exc.InnerException == null ? exc.Message : exc.InnerException.ToString()));
            }

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        //批量删除
        [HttpPost]
        public ActionResult DeleteSome(string ids)
        {
            ShowResultModel res = new ShowResultModel();
            try
            {
                string newids = ids.Substring(0, ids.Length - 1);
                string[] arrids = newids.Split('|');
                _AdminLogService.DeleteSome(arrids);
                res.IsSuccess = true;
                res.TipMsg = "删除成功!";
            }
            catch (Exception e)
            {
                res.TipMsg = e.Message;
                LoggerHelper.Log("【批量删除系统管理员日志】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (e.InnerException == null ? e.Message : e.InnerException.ToString()));
            }
            return Json(res);
        }

        //指定时间批量清除
        [HttpPost]
        public ActionResult CleareBeforeDate(DateTime dt)
        {
            ShowResultModel res = new ShowResultModel();
            try
            {
                _AdminLogService.DeleteBeforeDate(dt);
                res.IsSuccess = true;
                res.TipMsg = "清除指定日期前的日志成功！";//ResultMsg.DelOK;
            }
            catch (Exception e)
            {
                res.TipMsg = e.Message;
                LoggerHelper.Log("【批量清除系统管理员日志】出错，系统操作管理员：" + _currentAdmin.AdminInfo.SAName + "，错误原因:" + (e.InnerException == null ? e.Message : e.InnerException.ToString()));
            }
            return Json(res);
        }
    }
}
