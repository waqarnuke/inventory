using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Common;
using Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> _dbSet;
        private readonly StoreContext _context;
        public Repository(StoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _context.Products.Include(c => c.Category).Include(c=>c.CategoryId).Include(x=>x.Photos);
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public Task<T> Get(Expression<Func<T,bool>> filter, string? includeProperties = null)
        {
            //return _context.Set<T>().FindAsync(id)
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includporp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includporp);
                }
            }
            return query.FirstOrDefaultAsync(); 
        }
        
        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null)
        {
            //return _context.Set<T>(),ToListAsync();
            IQueryable<T> query = _dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includporp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includporp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllById(Expression<Func<T,bool>> filter,string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if(filter !=null)
            {
                query = query.Where(filter);
            }
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);   
                }
            }

            return await query.ToListAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);  
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();  
        }

        public async Task<PagedResult<T>> GetPagination(int index, int size, string orderBy = null, bool ascending = true, 
                                                        string includeProperties = null,Expression<Func<T,bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            // 🔎 Apply filter (searching)
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // 🧮 Total count BEFORE pagination
            int totalCount = await query.CountAsync();

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includporp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includporp);
                }
            }

            // 🔃 Sorting
            if (!string.IsNullOrEmpty(orderBy))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, orderBy);
                var lambda = Expression.Lambda(property, parameter);

                string methodName = ascending ? "OrderBy" : "OrderByDescending";
                var resultExp = Expression.Call(typeof(Queryable), methodName,
                    new Type[] { typeof(T), property.Type },
                    query.Expression, Expression.Quote(lambda));

                query = query.Provider.CreateQuery<T>(resultExp);
            }

            // 📄 Pagination
            var items = await query.Skip(index * size).Take(size).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = index,
                PageSize = size
            };
        }

        public void AddRange(IEnumerable<T> entity)
        {
            _dbSet.AddRange(entity);
        }
    }
}