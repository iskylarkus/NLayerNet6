using NLayerNet6.Core.Models;

namespace NLayerNet6.Core.Repositories
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<Category> GetCategoryByIdWithProductsAsync(int id);
    }
}
