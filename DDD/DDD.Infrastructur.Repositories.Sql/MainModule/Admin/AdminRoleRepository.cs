using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Infrastructur.Repositories.Sql.MainModule.Admin
{
    public class AdminRoleRepository : RepositoryBase<AdminRole>, IAdminRoleRepository
    {
        public AdminRoleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

    }
}
