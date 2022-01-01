using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace _0_Framework.Domain
{
    public interface IRepository<TKey, T> where T : class
    {
        void Create(T entity);
        void SaveChanges();
        List<T> GetList();
        T Get(TKey id);
        bool Exist(Expression<Func<T, bool>> expression);
    }
}