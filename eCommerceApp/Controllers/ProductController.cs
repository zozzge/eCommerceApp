using eCommerceApp.Models;
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
            List<Product> products = (List<Product>)_productService.GetAllProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.Identity.Name;
            // Implement add to cart logic
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = User.Identity.Name;
            // Implement remove from cart logic
            return RedirectToAction("Index");
        }
    }
}
