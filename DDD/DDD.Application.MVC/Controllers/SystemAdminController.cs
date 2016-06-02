using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DDD.Application.Model.DTO;
using DDD.Application.Model.VO;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.Common;
using DDD.Domain;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;
using DDD.Utility;
using Webdiyer.WebControls.Mvc;
using ChangePassword = DDD.Application.Model.DTO.ChangePassword;

namespace DDD.Application.MVC.Controllers
{
    //系统管理员管理
    public class SystemAdminController : BaseAdminControllers
    {
        //系统管理员
        private readonly ISystemAdminService _sysadminService;
        //管理员角色
        private readonly IAdminRoleService _adminRoleService;
        private readonly CurrentAdmin _curradmin;
        public SystemAdminController(ISystemAdminService sysadminService, IAdminRoleService adminRoleService, CurrentAdmin curradmin)
        {
            this._sysadminService = sysadminService;
            this._adminRoleService = adminRoleService;
            this._curradmin = curradmin;
        }
        //主页
        [HttpGet]
        public ActionResult Index()
        {
            SelectItems();
            return View();
        }
        //添加跳转
        [HttpGet]
        public ActionResult Add()
        {
            //读取角色信息
            ViewBag.RoleCheckString = _adminRoleService.GetAllRoletoCheck(new List<AdminRole>(), false);
            return View();
        }
        //添加功能
        [HttpPost]
        public ActionResult Add(SystemAdminRegister sa, string CheckRoleIds)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    SystemAdmin sad = new SystemAdmin() { Email = sa.Email, SANickName = sa.SANickName, SAName = sa.SAName, SAPwd = StrUtil.EncryptPassword(sa.Password, "MD5"), SASex = sa.SASex, SAMobileNo = sa.SAMobileNo, LoginTimes = 0, CurrentIP = "未登录", LastIP = "未登录", IsEnable = sa.IsEnable };
                    _sysadminService.AddSysAdmin(sad, CheckRoleIds);
                    srm.TipMsg = "新增管理员成功!<br/><br/><input type='button' class='box-buttonadd' value='继续添加'  onclick='window.document.getElementById(&#34;lhgfrm_divaddSysAdmin&#34;).contentWindow.Goon();'/><input type='button' class='box-buttonclose' value='取消' onclick='window.document.getElementById(&#34;lhgfrm_divaddSysAdmin&#34;).contentWindow.Undo()'/>";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
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
        public ActionResult Edit(int id)//said
        {
            SystemAdmin admin = _sysadminService.GetByCondition(new DirectSpecification<SystemAdmin>(sa => sa.SAID == id));
            SystemAdminUpdate sar = new SystemAdminUpdate() { SANickName = admin.SANickName, SAMobileNo = admin.SAMobileNo, SASex = admin.SASex, Email = admin.Email, SAID = admin.SAID, Password = admin.SAPwd, IsEnable = admin.IsEnable, SAName = admin.SAName };
            IEnumerable<AdminRole> listcheckrole = admin.AdminRoles.ToList();
            ViewBag.RoleCheckString = _adminRoleService.GetAllRoletoCheck(listcheckrole, false);
            return View(sar);
        }
        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="sa"></param>
        /// <param name="CheckRoleIds"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(SystemAdminUpdate sa, string CheckRoleIds, string oldPassword)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    string pwd = string.Empty;
                    if (oldPassword.Equals(sa.Password))
                        pwd = sa.Password;
                    else
                        pwd = StrUtil.EncryptPassword(sa.Password, "MD5");

                    SystemAdmin sad = new SystemAdmin() { Email = sa.Email, SANickName = sa.SANickName, SAName = sa.SAName, SAPwd = pwd, SASex = sa.SASex, SAMobileNo = sa.SAMobileNo, LoginTimes = 0, CurrentIP = "null", LastIP = "null", SAID = sa.SAID, IsEnable = sa.IsEnable };
                    _sysadminService.UpdateSysAdmin(sad, CheckRoleIds);
                    srm.TipMsg = "修改管理员成功!";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }
        //绑定下拉列表
        void SelectItems()
        {
            IEnumerable<SelectListItem> listitem = new[] { new SelectListItem { Value = "-1", Text = "所有角色" } }.Concat(_adminRoleService.GetAll().Select(ar => new SelectListItem
            {
                Text = ar.ARName,
                Value = ar.ARID.ToString().Trim()
            }));
            //指定下拉被选中
            ViewBag.RoleList = listitem;
        }
        /// <summary>
        /// 读取数据 分页 搜索
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="arid">角色id</param>
        /// <param name="nam">帐号</param>
        /// <param name="nknam">姓名</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSysadminList(int? pageIndex, int? pageSize, int? arid, string nam, string nknam)
        {
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? 10;
            arid = arid ?? -1;
            nam = nam ?? "";
            nknam = nknam ?? "";

            PageData<SystemAdmin> aList = _sysadminService.Search(pageIndex.Value, pageSize.Value, nam, nknam, arid.Value);
            PagedList<SystemAdmin> pList = null;
            if (aList.DataList != null)
            {
                pList = new PagedList<SystemAdmin>(aList.DataList, aList.CurrentPageIndex, pageSize.Value, aList.TotalCount);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_List", pList);
            }
            return RedirectToAction("Index");
        }
        //删除功能
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ShowResultModel srm = new ShowResultModel();
            try
            {
                _sysadminService.DeleteSysAdmin(id);
                srm.IsSuccess = true;
                srm.TipMsg = "删除成功!";
                srm.ReDirectUrl = "GetSysadminList";
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception ex)
            {
                srm.TipMsg = ex.Message;
            }
            return Json(srm);
        }

        #region 个人管理功能

        void AdminItems()
        {
            List<string> listrolename = _curradmin.AdminRoles.Select(x => x.ARName).ToList();
            string names = "无角色";
            if (listrolename != null && listrolename.Count > 0)
            {
                names = string.Empty;
                for (int i = 0; i < listrolename.Count; i++)
                {
                    if (i == listrolename.Count - 1)
                        names += listrolename[i];
                    else
                        names += listrolename[i] + "，";
                }
            }
            ViewBag.RoleNameList = names;
        }

        public ActionResult AccManIndex()
        {
            AdminItems();
            return View(_curradmin.AdminInfo);
        }

        //修改密码跳转
        [HttpGet]
        public ActionResult ChangePwd()
        {
            ChangePassword cp = new ChangePassword() { SAID = _curradmin.AdminInfo.SAID };
            return View(cp);
        }
        //修改密码功能
        [HttpPost]
        public ActionResult ChangePwd(ChangePassword cp)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    SystemAdmin sys = _sysadminService.GetByCondition(new DirectSpecification<SystemAdmin>(sa => sa.SAID == cp.SAID));
                    _sysadminService.ChangePwd(sys, cp.Password, cp.OldPassword);
                    srm.TipMsg = "修改密码成功!";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }
        //个人资料读取
        [HttpGet]
        public ActionResult AccManagerInfo(int id)
        {
            AdminItems();
            return PartialView("AccManager", _curradmin.AdminInfo);
        }
        //个人资料修改跳转
        [HttpGet]
        public ActionResult EditCurr()
        {
            SystemAdminUpdate adminu = new SystemAdminUpdate() { SANickName = _curradmin.AdminInfo.SANickName, Password = "******", IsEnable = false, SAID = _curradmin.AdminInfo.SAID, Email = _curradmin.AdminInfo.Email, SAMobileNo = _curradmin.AdminInfo.SAMobileNo, SASex = _curradmin.AdminInfo.SASex, SAName = _curradmin.AdminInfo.SAName };
            return View(adminu);
        }
        //个人资料修改功能
        [HttpPost]
        public ActionResult EditCurr(SystemAdminUpdate sau)
        {
            if (!Request.IsAjaxRequest())
                return Content("操作失败,你的浏览器禁用了Javascript脚本!");
            ShowResultModel srm = new ShowResultModel();
            if (ModelState.IsValid)
            {
                try
                {
                    SystemAdmin sad = new SystemAdmin() { Email = sau.Email, SANickName = sau.SANickName, SAName = _curradmin.AdminInfo.SAName, SAPwd = "null", SASex = sau.SASex, SAMobileNo = sau.SAMobileNo, LoginTimes = 0, CurrentIP = "null", LastIP = "null", SAID = sau.SAID, IsEnable = false };
                    _sysadminService.EditCurr(sad);
                    srm.TipMsg = "修改个人资料成功!";
                    srm.IsSuccess = true;
                }
                catch (InvalidOperationException ex)
                {
                    srm.TipMsg = ex.Message;
                }
                catch (Exception ex)
                {
                    srm.TipMsg = ex.Message;
                }
            }
            else
            {
                srm.TipMsg = "数据有效性验证失败!";
            }
            return Json(srm);
        }
        #endregion

        //是/否 禁用
        [HttpPost]
        public ActionResult Enable(string ids, bool isEnable)
        {
            ShowResultModel srm = new ShowResultModel();
            try
            {
                _sysadminService.Enable(ids, isEnable);
                srm.IsSuccess = true;
                srm.TipMsg = "操作成功!";
                srm.ReDirectUrl = "GetSysadminList";
            }
            catch (InvalidOperationException ex)
            {
                srm.TipMsg = ex.Message;
            }
            catch (Exception e)
            {
                srm.TipMsg = e.Message;
            }
            return Json(srm);
        }
    }
}
