using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain;
using DDD.Domain.Specification;

namespace DDD.Infrastructur.Repositories
{
    public interface IRepository<T, TPageData> where T : class,IAggregateRoot, new()
    {
        /// <summary>
        /// 数据分页的方法
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="specification">数据查询条件表达式</param>
        /// <param name="orderByExpression">排序的条件表达式</param>
        /// <param name="IsDESC">是否为倒序</param>
        /// <param name="AsNoTracking">DbContext是否不跟踪查询出的实体</param>
        /// <returns></returns>
        TPageData FindAll<S>(int PageIndex, int PageSize, ISpecification<T> specification, Expression<Func<T, S>> orderByExpression, bool IsDESC, bool? AsNoTracking = true);

        /// <summary>
        /// 得到所有数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll(bool? AsNoTracking = true);

        /// <summary>
        /// 根据条件表达式取得相关数据
        /// </summary>
        /// <param name="specification">Lambda表达式</param>
        /// <returns></returns>
        IEnumerable<T> GetMany(ISpecification<T> specification, bool? AsNoTracking = true);

        /// <summary>
        /// 取得记录数
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// 根据条件取得记录数
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        int GetCount(ISpecification<T> specification);

        /// <summary>
        /// 根据条件表达式取得指定条数的数据
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="TopN">返回的数据条数</param>
        /// <param name="specification">Lambda表达式</param>
        /// <param name="orderByExpression">Lambda表达式</param>
        /// <param name="IsDESC"></param>
        /// <returns></returns>
        IEnumerable<T> GetListByTopN<S>(int TopN, ISpecification<T> specification, Expression<Func<T, S>> orderByExpression, bool IsDESC, bool? AsNoTracking = true);

        /// <summary>
        /// 返回指定条件的数据   最多返回一条
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        T GetByCondition(ISpecification<T> specification, bool? AsNoTracking = false);

        /// <summary>
        /// 返回指定标识的实体
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <returns></returns>
        T GetByKey(object key);

        /// <summary>
        /// 将实体数据保存到数据库中
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Add(T entity);


        void AddBatch(IEnumerable<T> entities);

        /// <summary>
        /// 更新数据数据库中的一条数据  根据主键值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Modify(T entity);

        /// <summary>
        /// 更新实体的指定属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        void Update(T entity, params Expression<Func<T, object>>[] properties);

        /// <summary>
        /// 修改逻辑删除标记
        /// </summary>
        /// <param name="entity"></param>
        void UpdateIsDel(T entity);


        /// <summary>
        /// 从数据库中删除数据   仅用于同一上下文的集合对象有效
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Remove(T entity);

        /// <summary>
        /// 删除指定条件的数据
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        void Remove(ISpecification<T> specification);

        /// <summary>
        /// 判断指定条件的数据是否存在
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        bool Exists(ISpecification<T> specification);

        //跟踪实体
        IEnumerable<T> GetWithRawSql(string query, params object[] parameters);

        //不会跟踪实体
        IEnumerable<T> GetwhithdbSql(string query, params object[] parameters);
    }
}
