using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DDD.Domain;
using DDD.Infrastructur.Repositories;

namespace DDD.Application.Service
{
    /// <summary>
    /// 应用层服务实现抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseFrameworkServices<T> : IBaseServices<T>
        where T : class, IAggregateRoot, new()
    {
        protected readonly IRepository<T, PageData<T>> repository;
        protected readonly IUnitOfWorkFramework unitOfWork;

        protected BaseFrameworkServices(IUnitOfWorkFramework _unitOfWork, IRepository<T, PageData<T>> _Repository)
        {
            this.unitOfWork = _unitOfWork;
            this.repository = _Repository;
        }

        #region IBaseServices<T> 成员

        public virtual PageData<T> FindAll<S>(int PageIndex, int PageSize, Domain.Specification.ISpecification<T> specification, System.Linq.Expressions.Expression<Func<T, S>> orderByExpression, bool IsDESC, bool? AsNoTracking = true)
        {
            return this.repository.FindAll<S>(PageIndex, PageSize, specification, orderByExpression, IsDESC, AsNoTracking);
        }

        public virtual IEnumerable<T> GetAll(bool? AsNoTracking = true)
        {
            return this.repository.GetAll(AsNoTracking);
        }

        public virtual IEnumerable<T> GetMany(Domain.Specification.ISpecification<T> specification, bool? AsNoTracking = true)
        {
            return this.repository.GetMany(specification, AsNoTracking);
        }

        public virtual IEnumerable<T> GetListByTopN<S>(int TopN, Domain.Specification.ISpecification<T> specification, System.Linq.Expressions.Expression<Func<T, S>> orderByExpression, bool IsDESC, bool? AsNoTracking = true)
        {
            return this.repository.GetListByTopN<S>(TopN, specification, orderByExpression, IsDESC, AsNoTracking);
        }

        public virtual T GetByCondition(Domain.Specification.ISpecification<T> specification, bool? AsNoTracking = false)
        {
            return this.repository.GetByCondition(specification, AsNoTracking);
        }

        public virtual T GetByKey(object key)
        {
            return this.repository.GetByKey(key);
        }

        public virtual void Add(T entity)
        {
            this.repository.Add(entity);
            this.unitOfWork.Commit();
        }

        public virtual void Modify(T entity)
        {
            this.repository.Modify(entity);
            this.unitOfWork.Commit();
        }

        public void Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            this.repository.Update(entity, properties);
            this.unitOfWork.Commit();
        }

        public virtual void Remove(T entity)
        {
            this.repository.Remove(entity);
            this.unitOfWork.Commit();
        }

        public virtual void Remove(Domain.Specification.ISpecification<T> specification)
        {
            this.repository.Remove(specification);
            this.unitOfWork.Commit();
        }

        public virtual bool Exists(Domain.Specification.ISpecification<T> specification)
        {
            return this.repository.Exists(specification);
        }


        public virtual void SaveChanges()
        {
            this.unitOfWork.Commit();
        }




        public int GetCount()
        {
            return this.repository.GetCount();
        }

        public int GetCount(Domain.Specification.ISpecification<T> specification)
        {
            return this.repository.GetCount(specification);
        }

        #endregion
    }
}
