using Core.Interface;
namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly StoreContext _context;
        public ICategoryRepository categoryRepository {   get; private set; }
        public IProductRepository productRepository {   get; private set; }
        public UnitOfWork(StoreContext context)
        {
            _context = context;
            categoryRepository = new CategoryRepository(_context);
            productRepository = new ProductRepository(_context);
        }
        public async Task<int> Save()
        {
           return await _context.SaveChangesAsync();
        }
    }
}