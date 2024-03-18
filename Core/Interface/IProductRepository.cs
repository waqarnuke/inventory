using Core.Entities;

namespace Core.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}