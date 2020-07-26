using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangman.Models;
using Hangman.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hangman.Repository
{
    /**
    * Django-like model implementation of a repository (async).
    */
    public class HangmanRepositoryAsync<T> : IHangmanRepositoryAsync<T> where T : BaseEntity
    {
        private readonly HangmanDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public HangmanRepositoryAsync(HangmanDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async ValueTask<T?> GetById(Guid id) => await _dbSet.FindAsync(id);  // FindAsync uses primary key -> 1 result

        public async ValueTask<T?> Get(Expression<Func<T, bool>> filterPredicate) => await _dbSet.SingleOrDefaultAsync(filterPredicate); // 1 result or raise

        public async Task<IEnumerable<T>> All()
        {
            return await _dbSet.ToListAsync();
        }

        public async ValueTask<IEnumerable<T>> Filter(Expression<Func<T, bool>>? filterPredicate = null)
        {
            if (filterPredicate == null)
            {
                return await _dbSet.ToListAsync();
            }
            
            return await _dbSet.Where(filterPredicate).ToListAsync();
        }

        public async Task Save(T entity)
        {
            await _dbSet.AddAsync(entity); 
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            
            return await _dbSet.CountAsync(predicate);
        }
    }
}