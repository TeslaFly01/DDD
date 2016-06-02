using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Model.DTO
{
    public class SystemAdminUpdate
    {
        public int SAID { get; set; }
        /// <summary>
        /// 管理员姓名
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "姓名")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}位字符!")]
        public string SANickName { get; set; }

        [Required]
        [Display(Name = "帐号")]
        public string SAName { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "{0}必须在{2}-{1}位字符之间!", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "性别")]
        public bool SASex { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "电话")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}位字符!")]
        public string SAMobileNo { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "电子邮箱")]
        [RegularExpression(@"^[-0-9a-zA-Z~!$%^&*_=+}{\'?]+(\.[-0-9a-zA-Z~!$%^&*_=+}{\'?]+)*@([0-9a-zA-Z_][-0-9a-zA-Z_]*(\.[-0-9a-zA-Z_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[0-9a-zA-Z_][-0-9a-zA-Z_]*)|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$", ErrorMessage = "{0}格式不对!")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "是否启用")]
        public bool IsEnable { get; set; }
    }
}
