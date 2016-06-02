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
    /// 管理员角色
    /// </summary>
    public class AdminRole : EntityBase, IAggregateRoot
    {
        public AdminRole()
            : base()
        {
            this.ARID = -1;
            this.ARName = string.Empty;
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
                return this.ARID;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ARID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空!")]
        [Display(Name = "角色名")]
        [StringLength(100)]
        public string ARName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "角色描述")]
        public string Description { get; set; }

        public virtual ICollection<SystemAdmin> SystemAdmins { get; set; }
        public virtual ICollection<AdminRole_Module> AdminRole_Modules { get; set; }
    }
}
