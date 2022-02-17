using Microsoft.AspNetCore.Mvc;
using NLayerNet6.Core.Services;

namespace NLayerNet6.Web.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductWithCategory());
        }
    }
}
