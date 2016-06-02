using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructur.Repositories.Sql
{
    /// <summary>
    /// 数据库工厂框架接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDatabaseFrameworkFactory<out T> : IDisposable
        where T : DbContext, new()
    {
        T Get();
    }
}
