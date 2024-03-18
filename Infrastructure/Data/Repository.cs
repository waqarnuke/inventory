using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    }
}