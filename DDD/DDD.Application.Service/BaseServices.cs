using DDD.Domain;
using DDD.Infrastructur.Repositories;

namespace DDD.Application.Service
{
    /// <summary>
    /// 基础业务应用层服务实现抽象父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseServices<T> : BaseFrameworkServices<T>
        where T : class, IAggregateRoot, new()
    {

        protected BaseServices(IUnitOfWork _unitOfWork, IRepository<T, PageData<T>> _Repository)
            : base(_unitOfWork, _Repository)
        {
        }

    }
}
