using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DDD.Infrastructure.CrossCutting.Common;

namespace DDD.Application.MVC.Core.Controllers.Validate
{
    public class PasswordValiController:BaseControllers
    {
        public ActionResult CheckPassword(string Password, string CUName, string LoginName)
        {
            bool Flag = true;
            if (SystemHelper.CanNotUsePassword() != null)
            {
                if (SystemHelper.CanNotUsePassword().Any(item => item.Equals(Password)))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (Password.Equals(CUName))
                    return Json(false, JsonRequestBehavior.AllowGet);
                if (Password.Equals(LoginName))
                    return Json(false, JsonRequestBehavior.AllowGet);
                int y = 0;
                for (int i = 0; i < Password.Length; i++)
                {
                    //for (int k = i+1; k < Password.Length; k++)
                    //{
                    if (Password[i].Equals(Password[0]))
                        y++;
                    // }
                }
                if (y == Password.Length)
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(Flag, JsonRequestBehavior.AllowGet);
        }
    }
}
