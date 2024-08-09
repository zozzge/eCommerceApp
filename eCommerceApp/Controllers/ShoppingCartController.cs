using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Controllers
{
    public class ShoppingCartController:Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.Identity.Name;  // Assuming the user is authenticated and their username is used as UserId
            var cart = _shoppingCartService.GetCartByUserId(userId);
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.Identity.Name;  // Assuming the user is authenticated and their username is used as UserId
            _shoppingCartService.AddItemToCart(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.Identity.Name;  // Assuming the user is authenticated and their username is used as UserId
            _shoppingCartService.RemoveItemFromCart(userId, cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var userId = User.Identity.Name;  // Assuming the user is authenticated and their username is used as UserId
            _shoppingCartService.UpdateItemQuantity(userId, cartItemId, quantity);
            return RedirectToAction("Index");
        }
    }
}
