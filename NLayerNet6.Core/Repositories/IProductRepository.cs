using NLayerNet6.Core.Models;

namespace NLayerNet6.Core.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<List<Product>> GetProductWithCategory();
    }
}
