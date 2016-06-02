using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Infrastructur.Repositories.Sql.MainModule.Admin
{
    public class AdminRole_ModuleRepository : RepositoryBase<AdminRole_Module>, IAdminRole_ModuleRepository
    {
        public AdminRole_ModuleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public override void Remove(AdminRole_Module entity)
        {
            base.DataContext.AttachUpdated<AdminRole_Module>(entity);
            base.Remove(entity);
        }
    }
}
