using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;
using NLayerNet6.Web.Services;

namespace NLayerNet6.Web.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        [NonAction]
        public async Task<SelectList> GetAllCategoriesAsync(int id = 0)
        {
            var categories = await _categoryApiService.GetAllAsync();

            var categoryDto = new List<CategoryDto>();
            categoryDto.Add(new CategoryDto { Id = 0, Name = "Lütfen Seçiniz..." });
            categoryDto.AddRange(categories.ToList());

            return new SelectList(categoryDto, "Id", "Name", id);
        }

        [HttpGet("Save")]
        public async Task<IActionResult> Save()
        {
            ViewBag.Categories = await GetAllCategoriesAsync();

            return View();
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.SaveAsync(productDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await GetAllCategoriesAsync(productDto.CategoryId);

            return View();
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var productDto = await _productApiService.GetByIdAsync(id);

            ViewBag.Categories = await GetAllCategoriesAsync(productDto.CategoryId);

            return View(productDto);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await GetAllCategoriesAsync(productDto.CategoryId);

            return View(productDto);
        }

        [HttpGet("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _productApiService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
