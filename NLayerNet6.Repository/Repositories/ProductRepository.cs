using Microsoft.EntityFrameworkCore;
using NLayerNet6.Core.Models;
using NLayerNet6.Core.Repositories;

namespace NLayerNet6.Repository.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<List<Product>> GetProductWithCategory()
        {
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
