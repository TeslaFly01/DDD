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
    /// 管理员角色关联
    /// </summary>
    public class Admin_Role : EntityBase, IAggregateRoot
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
        /// 管理员id
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [Required]
        public int SAID { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        [Column(Order = 1)]
        [Key]
        public int ARID { get; set; }
    }
}
