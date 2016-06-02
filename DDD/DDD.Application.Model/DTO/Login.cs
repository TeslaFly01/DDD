using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.MVC.Core.Filters;

namespace DDD.Application.Model.DTO
{
    /// <summary>
    /// 登录
    /// </summary>
    public class Login
    {

        [Required(ErrorMessage = "帐号不能为空!")]
        [Display(Name = "账号")]
        public string SAName { get; set; }

        [Required(ErrorMessage = "密码不能为空!")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string SAPwd { get; set; }

        [Required(ErrorMessage = "验证码不能为空!")]
        [CustomRemote("CheckValiCode_Admin", "LoginVali", "", ErrorMessage = "验证码不正确！")]
        [Display(Name = "验证码")]
        public string ValidationCode { get; set; }

        //[Required]
        //public bool IsRememberMe { get; set; }
    }
}
