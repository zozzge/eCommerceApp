using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using eCommerceApp.ViewModels;
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

        public HomeController(ILogger<HomeController> logger, ProductService productService,ShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            
        }

        public IActionResult Index()
        {
            List<Product> products = (List<Product>)_productService.GetAllProducts();

            
            return View(products);
        }

        public async Task<IActionResult> CheckCartAndRedirect()//string userId)
        {
            var userId = User.Identity.Name;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userCart = await _shoppingCartService.GetCartByUserIdAsync();
            if (userCart != null && userCart.Items.Any())
            {
                return RedirectToAction("CheckOut", "Checkout");
            }
            else
            {
                // Handle the case where the cart is null (e.g., return an error or redirect to a different page)
                return RedirectToAction("CartEmpty", "ShoppingCart");
            }
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


