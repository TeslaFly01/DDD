using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Domain.MainModule.Admin
{
    public interface IAdminRoleDomainService
    {
        /// <summary>
        /// 读取角色表中的功能菜单
        /// </summary>
        /// <param name="arid"></param>
        /// <returns></returns>
        List<AdminModule> GetRoleModules(int arid);
        /// <summary>
        /// 读取功能菜单中的子菜单  FID相关
        /// </summary>
        /// <param name="arid"></param>
        /// <param name="firstid"></param>
        /// <returns></returns>
        string GetCurrRoleSecondModule(int arid, int firstid);
        /// <summary>
        /// 更新权值
        /// </summary>
        /// <param name="arid"></param>
        /// <param name="amid"></param>
        /// <param name="wgtids"></param>
        void UpdateWeight(int arid, string CMIDWeight);
        /// <summary>
        /// 读取角色列表 如果参数(选中项)为空 则全部未选中
        /// </summary>
        /// <param name="checkids"></param>
        /// <returns></returns>
        string GetAllRoletoCheck(IEnumerable<AdminRole> checkids, bool disabled);
        /// <summary>
        /// 读取子功能菜单中的action 如果参数(选中项)为空 则全部未选中
        /// </summary>
        /// <param name="checkids"></param>
        /// <param name="arid"></param>
        /// <returns></returns>
        string GetActionCheck(IEnumerable<AdminAction> checkids, int arid);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="arid"></param>
        void DeleteRole(AdminRole ar);
    }
}
