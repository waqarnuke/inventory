using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class BrandRepository: Repository<Brand>, IBrandRepository
    {
        private readonly StoreContext _context;
        public BrandRepository(StoreContext context) : base(context)
        {
             _context = context;

        }

        public bool IsExists(int id)
        {
           return _context.Brands.Any(x => x.Id == id);        }

        public void Update(Brand brand)
        {
           _context.Brands.Update(brand);
        }
    }
}