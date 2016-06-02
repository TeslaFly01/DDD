using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Infrastructur.Repositories;

namespace DDD.Domain.MainModule.Admin
{
    public interface IAdminModuleRepository : IRepository<AdminModule, PageData<AdminModule>>
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="adm"></param>
        void Move(AdminModule adm);
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="amid"></param>
        /// <param name="isEnable"></param>
        void Enable(int amid, bool isEnable);
    }
}
