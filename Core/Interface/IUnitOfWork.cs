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
        IImageRepository ImageRepository {get;}
        IBuyingRepository buyingRepository {get;}
        IRegisterRepository registerRepository {get;}
        Task<int> Save();
    }
}