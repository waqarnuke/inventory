using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}