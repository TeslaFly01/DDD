using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Model.Entities.Admin
{
    /// <summary>
    /// 系统管理员
    /// </summary>
    public class SystemAdmin : EntityBase, IAggregateRoot
    {
        public SystemAdmin()
            : base()//构造函数赋值必填字段
        {
            this.SAID = -1;
            this.SAName = string.Empty;
            this.SAPwd = string.Empty;
            this.SANickName = string.Empty;
            this.SASex = false;
            this.SAMobileNo = string.Empty;
            this.RegTime = DateTime.Now;
            this.IsEnable = false;
            this.LastIP = string.Empty;
            this.LastTime = DateTime.Now;
            this.CurrentIP = string.Empty;
            this.CurrentTime = DateTime.Now;
            this.Email = string.Empty;
            this.LoginTimes = -1;
        }

        [NotMapped]
        public override bool IsDel
        {
            get
            {
                return base.IsDel;
            }
            set
            {
                base.IsDel = value;
            }
        }

        [NotMapped]
        public override object Key
        {
            get
            {
                return this.SAID;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SAID { get; set; }

        [Required]
        [Display(Name = "帐号")]
        [StringLength(20, ErrorMessage = "账户不能超过20位!")]
        public string SAName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "密码不能超过20位!")]
        public string SAPwd { get; set; }

        /// <summary>
        /// 管理员姓名
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        [StringLength(20, ErrorMessage = "姓名不能超过20位!")]
        public string SANickName { get; set; }

        [Required]
        [Display(Name = "性别")]
        public bool SASex { get; set; }

        [Required]
        [Display(Name = "电话")]
        [StringLength(20, ErrorMessage = "电话不能超过20位!")]
        public string SAMobileNo { get; set; }

        [Required]
        [Display(Name = "注册时间")]
        public DateTime RegTime { get; set; }

        [Required]
        public bool IsEnable { get; set; }

        /// <summary>
        /// 上次访问ip
        /// </summary>
        [Required]
        [Display(Name = "上次登录IP")]
        [StringLength(50)]
        public string LastIP { get; set; }

        [Required]
        [Display(Name = "上次登录时间")]
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 当前ip
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "当前登录IP")]
        public string CurrentIP { get; set; }

        [Required]
        [Display(Name = "当前登录时间")]
        public DateTime CurrentTime { get; set; }

        [Required]
        [Display(Name = "邮箱")]
        [RegularExpression(@"^[-0-9a-zA-Z~!$%^&*_=+}{\'?]+(\.[-0-9a-zA-Z~!$%^&*_=+}{\'?]+)*@([0-9a-zA-Z_][-0-9a-zA-Z_]*(\.[-0-9a-zA-Z_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[0-9a-zA-Z_][-0-9a-zA-Z_]*)|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$", ErrorMessage = "电子邮箱格式不对!")]
        public string Email { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Required]
        [Display(Name = "登录次数")]
        public int LoginTimes { get; set; }

        public virtual ICollection<AdminRole> AdminRoles { get; set; }
    }
}
