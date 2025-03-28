using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Common;

namespace Core.Interface
{
    public interface IRepository<T> where T :class
    {
        Task<IEnumerable<T>> GetAll(string? includeProperties = null);
        Task<IReadOnlyList<T>> GetAllById(Expression<Func<T,bool>> filter, string? includeProperties = null);
        Task<T> Get(Expression<Func<T,bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<PagedResult<T>> GetPagination(int index, int size, string orderBy = null, bool ascending = true, 
                                                        string includeProperties = null,Expression<Func<T,bool>> filter = null);
    }
}