using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructur.Repositories.Sql
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private DBContext dataContext;
        public DBContext Get()
        {
            return dataContext ?? (dataContext = new DBContext());
        }
        //protected override void DisposeCore()
        //{
        //    if (dataContext != null)
        //        dataContext.Dispose();
        //}

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (dataContext != null)
                        dataContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
