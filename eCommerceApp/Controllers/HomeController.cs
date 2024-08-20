using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace eCommerceApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly ShoppingCartService _shoppingCartService;
        private readonly ApplicationDbContext _context;


        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService;
        private string userId;

        public HomeController(ILogger<HomeController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
            
        }

        public IActionResult Index()
        {
            List<Product> products = (List<Product>)_productService.GetAllProducts();

            //foreach (var product in products)
            //{
            //    var cartItem = shoppingCartItem.FirstOrDefault(item => item.ProductId == product.Id);
            //    if (cartItem != null)
            //    {
            //        product.QuantityInCart = cartItem.Quantity;
            //    }
            //    else
            //    {
            //        product.QuantityInCart = 0;
            //    }
            //}

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


