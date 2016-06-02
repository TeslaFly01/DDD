using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.MainModule.Admin
{
    public interface IAdminRole_ModuleDomainService
    {
        /// <summary>
        /// 添加角色关联
        /// </summary>
        /// <param name="moduleids"></param>
        /// <param name="arid"></param>
        void AddList(string[] moduleids, int arid);
        /// <summary>
        /// 修改角色/功能关联表
        /// </summary>
        /// <param name="moduleids"></param>
        /// <param name="arid"></param>
        void UpdateList(string[] moduleids, int arid);
    }
}
