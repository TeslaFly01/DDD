using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.MainModule.Admin
{
    public interface IAdminActionDomainService
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="aaid"></param>
        /// <param name="Flag"></param>
        void Move(int aaid, bool Flag);
    }
}
