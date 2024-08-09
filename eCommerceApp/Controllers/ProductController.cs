using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Controllers
{
    public class ProductController:Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }
    }
}
