using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TwitterBot.Framework.Types;

namespace TwitterBot.Framework.Contracts.Data
{
    public interface IDocumentDbRepository<T> 
        where T : BaseType
    {
        Task<T> AddOrUpdateAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<bool> RemoveAsync(string id);
        Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> TopAsync(Expression<Func<T, bool>> predicate, int n);
        IEnumerable<T> GetTweetsByHashtags(string[] hashtags, DateTime notBeforeDate);
    }
}
