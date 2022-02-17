using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;
using NLayerNet6.Core.Services;

namespace NLayerNet6.Web.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductWithCategory());
        }

        [HttpGet("Save")]
        public async Task<IActionResult> Save()
        {
            var categories = await _categoryService.GetAllAsync();

            var categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.Categories = new SelectList(categoryDto, "Id", "Name");

            return View();
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();

            var categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.Categories = new SelectList(categoryDto, "Id", "Name");

            return View();
        }
    }
}
