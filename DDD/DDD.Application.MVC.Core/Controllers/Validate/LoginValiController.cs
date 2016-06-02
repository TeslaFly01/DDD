using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DDD.Application.MVC.Core.Controllers.Validate
{
    public class LoginValiController : BaseControllers
    {
        public ActionResult CheckValiCode_Admin(string ValidationCode)
        {
            bool chkFlag = false;
            if (this.Session["AdminLogonVerifyCode"] != null)
            {
                if (this.Session["AdminLogonVerifyCode"].ToString() == ValidationCode.ToLower())
                    chkFlag = true;
            }
            else return Json("验证码失效，请重新刷新验证码！", JsonRequestBehavior.AllowGet);
            if (!chkFlag) return Json("验证码不正确！", JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
