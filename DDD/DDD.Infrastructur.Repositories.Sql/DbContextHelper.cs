using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain;

namespace DDD.Infrastructur.Repositories.Sql
{
    public static class DbContextHelper
    {
        public static void AttachUpdated<T>(this DbContext obj, T objectDetached) where T : EntityBase
        {
            if (objectDetached == null) throw new ArgumentNullException();
            if (obj.Entry<T>(objectDetached).State == EntityState.Detached)
            {
                obj.Set<T>().Attach(objectDetached);
            }
        }
    }
}
