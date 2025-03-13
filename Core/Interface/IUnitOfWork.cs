using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IUnitOfWork
    {
        ICategoryRepository categoryRepository {get;}
        IProductRepository productRepository {get;}
        IModelRepository ModelRepository {get;}
        IItemRepository ItemRepository {get;}
        IBrandRepository BrandRepository {get;}
        IMobileNetworkRepository mobileNetworkRepository{get;}
        IStorageRepository StorageRepository{ get;}

        ILocationRepository locationRepository {get;}

        Task<int> Save();
    }
}