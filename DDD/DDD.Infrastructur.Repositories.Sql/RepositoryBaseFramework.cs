using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain;

namespace DDD.Infrastructur.Repositories.Sql
{
    /// <summary>
    /// 仓储层实现抽象基类
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    /// <typeparam name="D">DbContext</typeparam>
    public abstract class RepositoryBaseFramework<T, D> : IRepository<T, PageData<T>>
        where T : EntityBase, IAggregateRoot, new()
        where D : DbContext, new()
    {
        private D dataContext;
        protected readonly DbSet<T> dbset;

        protected RepositoryBaseFramework(IDatabaseFrameworkFactory<D> databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFrameworkFactory<D> DatabaseFactory
        {
            get;
            private set;
        }

        protected D DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        #region IRepository<T,PageData<T>> 成员

        public virtual PageData<T> FindAll<S>(int PageIndex, int PageSize, Domain.Specification.ISpecification<T> specification, System.Linq.Expressions.Expression<Func<T, S>> orderByExpression, bool IsDESC, bool? AsNoTracking = true)
        {
            var query = IsDESC
                     ?
                     dbset.Where(specification.SatisfiedBy()).OrderByDescending(orderByExpression)
                     :
                     dbset.Where(specification.SatisfiedBy()).OrderBy(orderByExpression);

            PageData<T> pageData = new PageData<T>();
            int quyCount = query.Count();
            if (quyCount > 0)
            {
                pageData.TotalCount = quyCount;
                int TotalPages = (int)Math.Ceiling(pageData.TotalCount / (double)PageSize);
                pageData.CurrentPageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;
                if (AsNoTracking == true)
                    pageData.DataList = query.Skip((pageData.CurrentPageIndex - 1) * PageSize).Take(PageSize).AsNoTracking().ToList();
                else
                    pageData.DataList = query.Skip((pageData.CurrentPageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            return pageData;
        }

        public virtual IEnumerable<T> GetAll(bool? AsNoTracking = true)
        {
            if (AsNoTracking == true) return dbset.AsNoTracking().ToList();
            return dbset.ToList();
        }

        public virtual IEnumerable<T> GetMany(Domain.Specification.ISpecification<T> specification, bool? AsNoTracking = true)
        {
            if (AsNoTracking == true) return dbset.Where(specification.SatisfiedBy()).AsNoTracking().ToList();
            return dbset.Where(specification.SatisfiedBy()).ToList();
        }

        public virtual IEnumerable<T> GetListByTopN<S>(int TopN, Domain.Specification.ISpecification<T> specification, System.Linq.Expressions.Expression<Func<T, S>> orderByExpression, bool IsDESC, bool? AsNoTracking = true)
        {
            var query = IsDESC
                 ?
                 dbset.Where(specification.SatisfiedBy()).OrderByDescending(orderByExpression).Take(TopN)
                 :
                 dbset.Where(specification.SatisfiedBy()).OrderBy(orderByExpression).Take(TopN);

            if (AsNoTracking == true) return query.AsNoTracking().ToList();
            return query.ToList();
        }

        public virtual T GetByCondition(Domain.Specification.ISpecification<T> specification, bool? AsNoTracking = false)
        {
            if (AsNoTracking == false)
                return dbset.Where(specification.SatisfiedBy()).FirstOrDefault<T>();
            else return dbset.Where(specification.SatisfiedBy()).AsNoTracking().FirstOrDefault<T>();
        }

        public virtual T GetByKey(object key)
        {
            return dbset.Find(key);
        }

        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        public virtual void AddBatch(IEnumerable<T> entities)
        {
            dbset.AddRange(entities);
        }

        public virtual void Modify(T entity)
        {
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            dataContext.AttachUpdated(entity);
            DbEntityEntry<T> entry = dataContext.Entry(entity);
            foreach (var selector in properties)
            { entry.Property(selector).IsModified = true; }
        }

        public virtual void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public virtual void Remove(Domain.Specification.ISpecification<T> specification)
        {
            IEnumerable<T> objects = dbset.Where<T>(specification.SatisfiedBy()).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }

        public virtual bool Exists(Domain.Specification.ISpecification<T> specification)
        {
            return dbset.Any(specification.SatisfiedBy());
        }


        public virtual void UpdateIsDel(T entity)
        {
            Update(entity, x => x.IsDel);
        }

        /*执行存储过程方法
         * var parameter = new SqlParameter
    {
        DbType = DbType.Int32,
        ParameterName = "cid",
        Value = id
    };
    //联表并延迟加载
    var result = (from p in this.Categories.SqlQuery("EXECUTE GetCategory @cid", parameter) select p).ToList();
    return result;

        */
        //跟踪实体
        public virtual IEnumerable<T> GetWithRawSql(string query, params object[] parameters)
        { return dbset.SqlQuery(query, parameters); }

        //不会跟踪实体
        public virtual IEnumerable<T> GetwhithdbSql(string query, params object[] parameters)
        { return dataContext.Database.SqlQuery<T>(query, parameters); }

        public int GetCount()
        {
            return dbset.Count();
        }

        public int GetCount(Domain.Specification.ISpecification<T> specification)
        {
            return dbset.Where(specification.SatisfiedBy()).Count();
        }

        #endregion
    }
}
