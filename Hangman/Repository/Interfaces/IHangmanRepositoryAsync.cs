using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangman.Models;

namespace Hangman.Repository.Interfaces
{
    public interface IHangmanRepositoryAsync<T> where T : BaseEntity
    {
        ValueTask<T> GetById(Guid id);
        Task<IEnumerable<T>> All();
        ValueTask<IEnumerable<T>> Filter(Expression<Func<T, bool>> filterPredicate = null);
        Task Save(T entity);
        Task Delete(T entity);
        Task Update(T entity);
        Task<int> Count(Expression<Func<T, bool>> predicate);
    }
}