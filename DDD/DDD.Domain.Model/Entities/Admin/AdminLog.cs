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
    /// 管理员操作日志
    /// </summary>
    public class AdminLog : EntityBase, IAggregateRoot
    {
        public AdminLog()
            : base()
        {
            this.LogID = -1;
            this.OptContent = string.Empty;
            this.OptTime = DateTime.Now;
            this.UserID = -1;
            this.UserName = string.Empty;
            this.UserNickName = string.Empty;
            this.IP = string.Empty;
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
                return this.LogID;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }


        /// <summary>
        /// 操作内容
        /// </summary>
        [Required]
        [StringLength(50)]
        public string OptContent { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string OptRemark { get; set; }

        [Required]
        public DateTime OptTime { get; set; }

        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string UserNickName { get; set; }


        [Required]
        [StringLength(50)]
        public string IP { get; set; }
    }
}
