using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain
{
    /// <summary>
    /// 平台工作单元基接口
    /// </summary>
    public interface IUnitOfWorkFramework
    {
        void Commit();
    }
}
