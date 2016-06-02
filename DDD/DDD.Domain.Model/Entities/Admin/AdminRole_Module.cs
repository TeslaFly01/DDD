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
    /// 管理员角色功能模块关联
    /// </summary>
    public class AdminRole_Module : EntityBase, IAggregateRoot
    {
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

        /// <summary>
        /// 角色id
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [Required]
        public int ARID { get; set; }

        /// <summary>
        /// 功能模块id
        /// </summary>
        [Required]
        [Column(Order = 1)]
        [Key]
        public int AMID { get; set; }

        /// <summary>
        /// 权重值
        /// </summary>
        [Required]
        public int Weights { get; set; }

        public virtual AdminRole adminRole { get; set; }
        public virtual AdminModule adminModule { get; set; }
    }
}
