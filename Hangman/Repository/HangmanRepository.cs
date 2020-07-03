using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hangman.Models;
using Microsoft.EntityFrameworkCore;

namespace Hangman.Repository
{
    /**
    * Generic repository of type T with many utility methods to perform Db operations
     * with the Db context (~ Django model).
    */
    public class HangmanRepository<T> where T : BaseEntity
    {
        private readonly HangmanDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public HangmanRepository(HangmanDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public T FindById(Guid id)
        {
            return _dbSet.SingleOrDefault(e => e.Id == id);
        }
        
        public IEnumerable<T> All()
        {
            return _dbSet.ToList();
        }
        
        public List<T> Filter(Expression<Func<T, bool>> filter = null)
        {
            var query = _dbSet.AsQueryable();
            
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            return query.ToList();
        }

        public T Save(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}