using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public interface IAdminRole_ModuleService : IBaseServices<AdminRole_Module>
    {   
        /// <summary>
        /// 添加角色关联
        /// </summary>
        /// <param name="moduleids">功能ids</param>
        /// <param name="arid">角色id</param>
        void AddList(string[] moduleids, int arid);
        /// <summary>
        /// 更新角色-功能关联表
        /// </summary>
        /// <param name="moduleids">功能ids</param>
        /// <param name="arid">角色id</param>
        void UpdateList(string[] moduleids, int arid);
    }
}
