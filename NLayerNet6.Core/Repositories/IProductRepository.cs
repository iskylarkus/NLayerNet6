using NLayerNet6.Core.Models;

namespace NLayerNet6.Core.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<List<Product>> GetProductWithCategory();
    }
}
