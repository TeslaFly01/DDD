using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;
using DDD.Infrastructur.Repositories;

namespace DDD.Domain.MainModule.Admin
{
    public interface IAdminRole_ModuleRepository : IRepository<AdminRole_Module, PageData<AdminRole_Module>>
    {
    }
}
