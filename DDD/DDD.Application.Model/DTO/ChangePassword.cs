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
    /// 修改密码
    /// </summary>
    public class ChangePassword
    {
        [Required]
        public int SAID { get; set; }

        public string CUName { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "{0}必须在{2}-{1}位字符之间!", MinimumLength = 6)]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [StringLength(50, ErrorMessage = "{0}必须在{2}-{1}位字符之间!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        [CustomRemote("CheckPassword", "PasswordVali", "", ErrorMessage = "密码太简单！", AdditionalFields = "CUName")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "与新密码不匹配!")]
        public string ConfirmPassword { get; set; }
    }
}
