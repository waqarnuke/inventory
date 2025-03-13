using Core.Interface;
namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly StoreContext _context;
        public ICategoryRepository categoryRepository {   get; private set; }
        public IProductRepository productRepository {   get; private set; }
        public IItemRepository itemRepository { get; private set; }
        public IImageRepository imageRepository { get; private set; }
        public IBuyingRepository buyingRepository { get; private set; }
        public IRegisterRepository registerRepository { get; private set; }
        public IBrandRepository brandRepository {   get; private set; }
        public ILocationRepository locationRepository {get; private set;}
        public IMobileNetworkRepository mobileNetworkRepository {get; private set;}
        public IStorageRepository storageRepository { get; private set;}
        public IModelRepository modelRepository {get; private set;}
        public ISaleRepository saleRepository {get; private set;}

        public UnitOfWork(StoreContext context)
        {
            _context = context;
            categoryRepository = new CategoryRepository(_context);
            productRepository = new ProductRepository(_context);
            itemRepository = new ItemRepository(_context);
            
            brandRepository = new BrandRepository(_context);
            mobileNetworkRepository = new MobileNetworkRepository(_context);
            storageRepository = new StorageRepository(_context);
            modelRepository = new ModelRepository(_context);
            locationRepository = new LocationRepository(_context);
            
            imageRepository = new ImageRepository(_context);
            buyingRepository = new BuyingRepository(_context);
            registerRepository = new RegisterRepository(_context);
            saleRepository = new SaleRepository(_context);
        }
        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}