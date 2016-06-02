using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Model.Entities.Admin
{
    public class Department : EntityBase, IAggregateRoot
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
