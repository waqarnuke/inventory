using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        
        private readonly StoreContext _context;
        public CategoryRepository(StoreContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}