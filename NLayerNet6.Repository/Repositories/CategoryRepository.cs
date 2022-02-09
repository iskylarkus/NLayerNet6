using Microsoft.EntityFrameworkCore;
using NLayerNet6.Core.Models;
using NLayerNet6.Core.Repositories;

namespace NLayerNet6.Repository.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Category> GetCategoryByIdWithProductsAsync(int id)
        {
            return await _context.Categories.Include(x => x.Products).Where(x => x.Id == id).SingleOrDefaultAsync();
        }
    }
}
