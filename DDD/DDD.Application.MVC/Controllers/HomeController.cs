using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DDD.Application.Model.DTO;
using DDD.Application.Model.VO;
using DDD.Application.MVC.Filters;
using DDD.Application.Service.BusinessService;
using DDD.Application.Service.BusinessService.Admin;
using DDD.Application.Service.CacheService;
using DDD.Application.Service.Common;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.Model.Entities.Admin;
using DDD.Infrastructure.CrossCutting.Common;
using DDD.Infrastructure.CrossCutting.IOC;
using DDD.Utility;
using DDD.Utility.ValidateCode;
using Microsoft.Practices.Unity;

namespace DDD.Application.MVC.Controllers
{
    public class HomeController : BaseAdminControllers
    {
        private readonly IUserInfoService _userInfoService;
        private readonly ISystemAdminService _systemAdminService;
        private readonly IAdminLogService _adminLogService;
        private readonly AdminCacheService _adminCacheService;

        public HomeController(IUserInfoService userInfoService, ISystemAdminService systemAdminService, IAdminLogService adminLogService, AdminCacheService adminCacheService)
        {
            _userInfoService = userInfoService;
            _systemAdminService = systemAdminService;
            _adminLogService = adminLogService;
            _adminCacheService = adminCacheService;
        }

        public ActionResult Index()
        {
            //var list=_userInfoService.GetList("A", 2);

            return View("~/Views/Index.cshtml");
        }

        public ActionResult IndexFrame()
        {
            return View("~/Views/IndexFrame.cshtml");
        }

        //[DonutOutputCache(Duration = 60)]
        public ActionResult Main()
        {
            return View("~/Views/Main.cshtml");
        }

        public ActionResult MainTop()
        {
            return PartialView("~/Views/_MainTop.cshtml");
        }

        public ActionResult Top()
        {
            return View("~/Views/Top.cshtml");
        }
        public ActionResult Left()
        {
            return View("~/views/Left.cshtml");
        }
        public ActionResult LeftMenu()
        {
            var curAdmin = UnityContainerFactory.Instance.CurrentContainer.Resolve<CurrentAdmin>();
            IEnumerable<AdminModule> adminModules = curAdmin.AdminModules;

            return PartialView("~/views/_LeftMenu.cshtml", adminModules.ToList());
        }

        [ExcAdminAuth]
        public ActionResult CallLeft()
        {
            return View("~/Views/CallLeft.cshtml");
        }

        [ExcAdminAuth]
        public ActionResult Foot()
        {
            return View("~/Views/Foot.cshtml");
        }

        [ExcAdminAuth]
        public ActionResult Login()
        {
            return View("~/Views/Login.cshtml");
        }

        //登录提交
        [ExcAdminAuth]
        [HttpPost]
        //[HandleErrorWithLog4net(HandleType = ErrorHandleType.ResponseWrite, ResponseMsg = "网络问题，请稍后再试！")]
        //[ValidateAntiForgeryToken(Salt = SystemHelper.AntiForgeryTokenSalt)]
        public ActionResult Login(Login model)
        {
            ShowResultModel rs = new ShowResultModel();

            if (ModelState.IsValid)
            {
                SystemAdmin user = _systemAdminService.GetByNameAndPassword(model.SAName, model.SAPwd);
                if (user != null)
                {
                    if (!user.IsEnable)
                    {
                        rs.TipMsg = "该账户已被禁用！";
                        return Json(rs);
                    }

                    rs.IsSuccess = true;

                    var userModules = _systemAdminService.GetsysAdminModule(user);
                    // user data:
                    var userDate = ";";
                    if (userModules != null && userModules.Any())
                    {
                        var ulist =
                            userModules.Where(x => !string.IsNullOrEmpty(x.FormRoleName))
                                       .Select(x => x.FormRoleName)
                                       .Distinct()
                                       .ToArray();
                        if (ulist.Any())
                            userDate = string.Join(",", ulist) + ";";
                    }

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        user.SAName,//user.Name
                        DateTime.Now,
                        DateTime.Now.Add(FormsAuthentication.Timeout),
                        false,//model.RememberMe,
                        // user data:
                        userDate
                        //new string[] { "admin", "corp" }.Aggregate((i, j) => i + "," + j) + ";"
                            + IPHelper.getIPAddr() + ";"
                        + user.SAID.ToString() + ";"
                        + user.SANickName
                        );

                    HttpCookie cookie = new HttpCookie(
                        FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(ticket));
                    cookie.HttpOnly = true;//不能通过客户端脚本访问cookie
                    Response.Cookies.Add(cookie);

                    //登录成功更新访问时间
                    _systemAdminService.UpdateLogonInfo(user);

                    _adminLogService.Log(user, "管理员登录", "帐号：" + user.SAName + " || 姓名：" + user.SANickName + " || 上次访问IP:" + user.LastIP + " || 上次访问时间:" + user.LastTime.ToString() + " || 当前访问IP:" + user.CurrentIP + " || 当前访问时间:" + user.CurrentTime.ToString() + " || 登录次数:" + user.LoginTimes.ToString());

                    _adminCacheService.Remove(AdminCacheService.SysAdmin_Current_prefix + user.SAName);
                    _adminCacheService.Add(AdminCacheService.SysAdmin_Current_prefix + user.SAName, user, TimeSpan.FromHours(2));
                }
                else
                {
                    rs.TipMsg = "用户名或密码错误！";
                }

            }
            else
            {
                rs.TipMsg = "数据有效性验证失败！";
            }
            return Json(rs);

        }

        [HttpGet]
        public ActionResult Logout()
        {
            bool logged = User.Identity.IsAuthenticated;
            if (!logged) return Content("未登录，不需要注销！");
            CurrentAdminEx curUser = new CurrentAdminEx();
            FormsAuthentication.SignOut();
            if (Request.Cookies["WquanAdminAuth"] != null)
            {
                Request.Cookies["WquanAdminAuth"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Request.Cookies["WquanAdminAuth"]);
            }
            Request.Cookies.Clear();
            Session.Clear();
            _adminCacheService.Remove(AdminCacheService.SysAdmin_Current_prefix + curUser.SAName);
            string js = "window.top.location='/';";
            return JavaScript(js);
        }

        [ExcAdminAuth]
        public ActionResult VerifyImage()
        {
            Random random = new Random();
            int i = random.Next() % 2;
            ValidateCodeType s1;
            if (i == 0) s1 = new ValidateCode_Style1();
            else s1 = new ValidateCode_Style14();
            string code = "6666";
            byte[] bytes = s1.CreateImage(out code);

            this.Session["AdminLogonVerifyCode"] = code.ToLower();
            // Response.Cache.SetCacheability(HttpCacheability.NoCache); //note:这样可能会对整个页面的缓存有影响，还是通过js提交参数防止缓存
            return File(bytes, @"image/jpeg");

        }
    }
}
