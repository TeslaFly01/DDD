using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Infrastructur.Repositories.Sql.MainModule.Admin
{
    public class AdminActionRepository : RepositoryBase<AdminAction>, IAdminActionRepository
    {
        public AdminActionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Move(AdminAction aat)
        {
            base.DataContext.AttachUpdated<AdminAction>(aat);
            var updateEntry = ((IObjectContextAdapter)base.DataContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(aat);
            updateEntry.SetModifiedProperty("SortFlag");
        }
    }
}
