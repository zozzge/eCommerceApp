using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eCommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService;
        public HomeController(ILogger<HomeController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            List<Product> products = (List<Product>)_productService.GetAllProducts();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
