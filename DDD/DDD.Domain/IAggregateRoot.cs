using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain
{
    /// <summary>
    /// 这是一个约束接口，申明一个实体是一个聚合根；
    /// 所有持久化实体都要实现该接口
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
    }
}
