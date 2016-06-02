using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Model.Entities.Admin
{
    public class UserInfo : EntityBase, IAggregateRoot
    {
        [NotMapped]
        public override object Key
        {
            get
            {
                return ID;
            }
        }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
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

        public int ID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int? Sex { get; set; }
        public int DeptId { get; set; }

        public int ScanFlag { get; set; }


        public virtual Department Department { get; set; }
    }
}
