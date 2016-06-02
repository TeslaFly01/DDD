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
    /// 管理员功能模块（菜单权限）
    /// </summary>
    public class AdminModule : EntityBase, IAggregateRoot
    {
        public AdminModule()
            : base()
        {
            this.AMID = -1;
            this.ModuleName = string.Empty;
            this.FID = -1;
            this.SortFlag = DateTime.Now;
            this.IsEnable = false;
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
                return this.AMID;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AMID { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "模块名称")]
        public string ModuleName { get; set; }

        [StringLength(100, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "页面地址")]
        public string PageUrl { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "所属父级")]
        public int FID { get; set; }

        public virtual AdminModule FAdminModule { get; set; }

        /// <summary>
        /// Form身份角色名称
        /// </summary>
        [StringLength(50, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "身份角色名称")]
        public string FormRoleName { get; set; }

        [Required]
        public DateTime SortFlag { get; set; }

        [StringLength(100, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "CSS样式")]
        public string CSSStyle { get; set; }

        [StringLength(100, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "图标")]
        public string Icon { get; set; }

        [Required]
        [Display(Name = "启用标志")]
        public bool IsEnable { get; set; }

        public virtual ICollection<AdminRole_Module> AdminRole_Modules { get; set; }

        public virtual ICollection<AdminAction> AdminActions { get; set; }

    }
}
