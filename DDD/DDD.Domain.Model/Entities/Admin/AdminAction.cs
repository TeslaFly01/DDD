using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Infrastructure.CrossCutting.Common.Validation;

namespace DDD.Domain.Model.Entities.Admin
{
    /// <summary>
    /// 管理员操作权限
    /// </summary>
    public class AdminAction : EntityBase, IAggregateRoot 
    {
        public AdminAction()
            : base()
        {
            this.AAID = -1;
            this.OptName = string.Empty;
            this.ControllerName = string.Empty;
            this.ActionName = string.Empty;
            this.Weight = -1;
            this.SortFlag = DateTime.Now;
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
                return this.AAID;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AAID { get; set; }

        [Required]
        public int AMID { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "权限名称")]
        public string OptName { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "Controller")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "{0}不能为空!")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}位字符!")]

        [Display(Name = "Action")]
        public string ActionName { get; set; }

        [StringLength(100, ErrorMessage = "{0}不能超过{1}位字符!")]
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Required]
        [TwoSqrt(ErrorMessage = "{0}必须是2的N次方!")]
        [Range(1, 32768, ErrorMessage = "{0}必须在{2}-{1}之间的正整数!")]
        [Display(Name = "权值")]
        public int? Weight { get; set; }

        [Required]
        public DateTime SortFlag { get; set; }
    }
}
