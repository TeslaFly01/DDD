using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Infrastructur.Repositories;

namespace DDD.Domain.MainModule.Admin
{
    public interface ISystemAdminRepository : IRepository<SystemAdmin, PageData<SystemAdmin>>
    {
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="said"></param>
        /// <param name="isEnable"></param>
        void Enable(int said, bool isEnable);
        //void UpdateSysAdmin(SystemAdmin admin, string[] roleids);
        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="admin"></param>
        void UpdateSysAdmin(SystemAdmin admin);
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
        /// 修改密码
        /// </summary>
        /// <param name="admin"></param>
        void ChangePwd(SystemAdmin admin, string newPwd);
        /// <summary>
        /// 根据管理员id读取当前登录ip
        /// </summary>
        /// <param name="said"></param>
        /// <returns></returns>
        string GetIPBysysAdmin(int said);
    }
}
