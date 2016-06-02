using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Domain.MainModule.Admin
{
    public interface IAdminModuleDomainService
    {
        /// <summary>
        /// 上/下排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        void Move(int amid, bool Flag);
        /// <summary>
        /// 读取功能的树形结构
        /// </summary>
        /// <param name="checkids"></param>
        /// <returns></returns>
        string GetCheckStr(IEnumerable<AdminRole_Module> checkids);
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="amid"></param>
        void Delete(AdminModule am);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        void DeleteList(string[] ids);
    }
}
