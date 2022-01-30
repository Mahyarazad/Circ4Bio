using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using _0_Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _0_Framework.Infrastructure
{
    public class RepositoryBase<TKey, T> : IRepository<TKey, T> where T : class
    {
        private readonly DbContext _dbContext;


        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Create(T entity)
        {
            _dbContext.Add<T>(entity);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        List<T> IRepository<TKey, T>.GetList()
        {
            return GetList();
        }

        public List<T> GetList()
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public T Get(TKey id)
        {
            return _dbContext.Find<T>(id);
        }

        public bool Exist(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Any(expression);
        }
    }
}