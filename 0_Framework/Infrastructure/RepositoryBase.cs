using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
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

        public async Task<List<T>> GetList()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public bool Exist(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Any(expression);
        }

        public Task<T> Get(TKey id)
        {
            return Task.FromResult(_dbContext.Set<T>().Find(id));
        }
    }
}