using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Domain.MainModule.Admin
{
    public interface ISystemAdminDomainService
    {
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="roleids"></param>
        void AddSysAdmin(SystemAdmin admin, string[] roleids);
        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="roleids"></param>
        void UpdateSysAdmin(SystemAdmin admin, string[] roleids);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="admin"></param>
        void ChangePwd(SystemAdmin admin, string newPwd, string oldPwd);
        /// <summary>
        /// 更新个人资料
        /// </summary>
        /// <param name="admin"></param>
        void EditCurr(SystemAdmin admin);
        /// <summary>
        /// 更新登录状态
        /// </summary>
        /// <param name="admin"></param>
        void UpdateLogonInfo(SystemAdmin admin);
        /// <summary>
        /// 读取当前管理员拥有的所有角色下的功能模块
        /// </summary>
        /// <param name="admin">当前登录管理员</param>
        /// <returns></returns>
        IEnumerable<AdminModule> GetsysAdminModule(SystemAdmin admin);
        /// <summary>
        /// 检查当前管理员是否登录 一致返回true 不一致返回false
        /// </summary>
        /// <param name="said">管理员id</param>
        /// <param name="currIP">当前登录ip</param>
        /// <returns></returns>
        bool CheckIsRepeatLogon(int said, string currIP);
    }
}
