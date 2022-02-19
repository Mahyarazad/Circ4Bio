using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _0_Framework.Domain
{
    public interface IRepository<TKey, T> where T : class
    {
        void Create(T entity);
        void SaveChanges();
        public Task<List<T>> GetList();
        public Task<T> Get(TKey id);
        bool Exist(Expression<Func<T, bool>> expression);
    }
}