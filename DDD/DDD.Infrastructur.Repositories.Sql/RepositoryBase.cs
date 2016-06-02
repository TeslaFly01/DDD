using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain;

namespace DDD.Infrastructur.Repositories.Sql
{
    /// <summary>
    /// 基础数据仓储实现抽象父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : RepositoryBaseFramework<T, DBContext>
        where T : EntityBase, IAggregateRoot, new()
    {

        protected RepositoryBase(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


    }
}
