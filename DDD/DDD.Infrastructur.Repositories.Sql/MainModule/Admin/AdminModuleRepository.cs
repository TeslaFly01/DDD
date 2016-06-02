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
    public class AdminModuleRepository : RepositoryBase<AdminModule>, IAdminModuleRepository
    {
        public AdminModuleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Move(AdminModule adm)
        {
            Update(adm, x => x.SortFlag);
        }

        public void Enable(int amid, bool isEnable)
        {
            var Enableadminmodule = new AdminModule() { AMID = amid, IsEnable = isEnable, ModuleName = "null" };
            base.dbset.Attach(Enableadminmodule);
            var updateEntry = ((IObjectContextAdapter)base.DataContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(Enableadminmodule);
            updateEntry.SetModifiedProperty("IsEnable");
        }
    }
}
