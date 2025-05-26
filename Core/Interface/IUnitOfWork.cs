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
        IItemRepository itemRepository {get;}
        IImageRepository imageRepository {get;}
        IBuyingRepository buyingRepository {get;}
        IRegisterRepository registerRepository {get;}
        IModelRepository modelRepository {get;}
        IBrandRepository brandRepository {get;}
        IMobileNetworkRepository mobileNetworkRepository{get;}
        IStorageRepository storageRepository{ get;}
        ILocationRepository locationRepository {get;}
        ISaleRepository saleRepository {get;}
        ISupplierRepository supplierRepository {get;}
        ICompanyRepository companyRepository {get;}
        IUserLocationAssignmentsRepository userLocationAssignmentsRepository {get;}
        IBuyRegisterRepository buyRegisterRepository {get;}
        ISaleRegisterRepository saleRegisterRepository {get;}
        Task<int> Save();
    }
}