using Microsoft.AspNetCore.Mvc;
using NLayerNet6.Core.Services;

namespace NLayerNet6.API.Controllers
{
    [Route("api/categories")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategoryByIdWithProducts(int id)
        {
            return CreateActionResult(await _categoryService.GetCategoryByIdWithProductsAsync(id));
        }
    }
}
