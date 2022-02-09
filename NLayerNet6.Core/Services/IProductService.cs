using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;

namespace NLayerNet6.Core.Services
{
    public interface IProductService:IService<Product>
    {
        Task<ResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory();
    }
}
