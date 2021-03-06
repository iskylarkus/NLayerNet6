using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;

namespace NLayerNet6.Core.Services
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<ResponseDto<CategoryWithProductsDto>> GetCategoryByIdWithProductsAsync(int id);
    }
}
