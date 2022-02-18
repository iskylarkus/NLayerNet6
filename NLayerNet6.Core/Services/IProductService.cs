using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;

namespace NLayerNet6.Core.Services
{
    public interface IProductService : IGenericService<Product>
    {
        Task<List<ProductWithCategoryDto>> GetProductWithCategory();
    }
}
