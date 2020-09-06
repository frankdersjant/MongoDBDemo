using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        IEnumerable<TEntity> GetAll();
        long Count();
        TEntity GetSingle(string id);
        TEntity GetSingleItemPredicate(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
    }
}
