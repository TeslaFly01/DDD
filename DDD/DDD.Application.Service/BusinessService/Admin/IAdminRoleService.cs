using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Application.Service.BusinessService.Admin
{
    public interface IAdminRoleService : IBaseServices<AdminRole>
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="arl">角色对象</param>
        /// <param name="moduleids">功能id数组</param>
        void AddRole(AdminRole arl, string moduleids);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="arl">角色</param>
        /// <param name="moduleids">功能ids</param>
        void UpdateRole(AdminRole arl, string moduleids);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="arid">角色id</param>
        void DeleteRole(int arid);

        /// <summary>
        /// 得到当前角色的功能一级菜单
        /// </summary>
        /// <param name="arid">角色id</param>
        /// <returns></returns>
        List<AdminModule> GetRoleModules(int arid);
        /// <summary>
        /// 得到当前角色的功能二级菜单
        /// </summary>
        /// <param name="arid">角色id</param>
        /// <param name="firstid">一级（功能）id</param>
        /// <returns></returns>
        string GetCurrRoleSecondModule(int arid, int firstid);
        /// <summary>
        /// 更新权值
        /// </summary>
        /// <param name="arid">角色id</param>
        /// <param name="amid">功能id</param>
        /// <param name="wgtids">权值数组</param>
        void UpdateWeight(int arid, string CMIDWeight);
        /// <summary>
        /// 读取角色
        /// </summary>
        /// <param name="checkids">角色项</param>
        /// <param name="disabled">是否选中</param>
        /// <returns></returns>
        string GetAllRoletoCheck(IEnumerable<AdminRole> checkids, bool disabled);
        /// <summary>
        /// 读取action
        /// </summary>
        /// <param name="checkids">选中的action项 </param>
        /// <param name="arid">角色id</param>
        /// <returns></returns>
        string GetActionCheck(IEnumerable<AdminAction> checkids,int arid);
    }
}
