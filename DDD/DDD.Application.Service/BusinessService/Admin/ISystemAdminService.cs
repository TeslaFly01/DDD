using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain;

namespace DDD.Application.Service.BusinessService.Admin
{
    public interface ISystemAdminService : IBaseServices<SystemAdmin>
    {
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="admin">管理员对象</param>
        /// <param name="roleids">角色ids</param>
        void AddSysAdmin(SystemAdmin admin, string roleids);
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="ids">管理员ids</param>
        /// <param name="isEnable">启用、禁用</param>
        void Enable(string ids, bool isEnable);
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="said">管理员id</param>
        void DeleteSysAdmin(int said);
        /// <summary>
        /// 搜索数据 分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="SAName">帐号</param>
        /// <param name="SANickName">姓名</param>
        /// <param name="adminRole">角色id</param>
        /// <returns></returns>
        PageData<SystemAdmin> Search(int PageIndex, int PageSize, string SAName, string SANickName, int ARID);
        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="admin">管理员对象</param>
        /// <param name="roleids">角色ids</param>
        void UpdateSysAdmin(SystemAdmin admin, string roleids);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="admin">管理员对象</param>
        void ChangePwd(SystemAdmin admin,string newPwd,string oldPwd);
        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <param name="admin">管理员对象</param>
        void EditCurr(SystemAdmin admin);
        /// <summary>
        /// 登录读取账户信息
        /// </summary>
        /// <param name="SAName">帐号</param>
        /// <param name="SAPwd">密码</param>
        /// <returns></returns>
        SystemAdmin GetByNameAndPassword(string SAName, string SAPwd);
        /// <summary>
        /// 更新登录成功后的信息  登录次数 当前登录ip、时间  上一次登录ip、时间
        /// </summary>
        /// <param name="admin">登录管理员对象</param>
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
