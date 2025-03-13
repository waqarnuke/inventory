using Core.Interface;
namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly StoreContext _context;
        public ICategoryRepository categoryRepository {   get; private set; }
        public IBrandRepository BrandRepository {   get; private set; }
        public ILocationRepository locationRepository {get; private set;}

        public IProductRepository productRepository {   get; private set; }
        public IItemRepository ItemRepository { get; private set; }

        public IMobileNetworkRepository mobileNetworkRepository {get; private set;}
        
        public IStorageRepository StorageRepository { get; private set;}

        public IModelRepository ModelRepository {get; private set;}

        public UnitOfWork(StoreContext context)
        {
            _context = context;
            categoryRepository = new CategoryRepository(_context);
            productRepository = new ProductRepository(_context);
            ItemRepository = new ItemRepository(_context);
            BrandRepository = new BrandRepository(_context);
            mobileNetworkRepository = new MobileNetworkRepository(_context);
            StorageRepository = new StorageRepository(_context);
            ModelRepository = new ModelRepository(_context);
            locationRepository = new LocationRepository(_context);
        }
        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}