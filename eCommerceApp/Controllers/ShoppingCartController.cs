using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace eCommerceApp.Controllers
{
    public class ShoppingCartController:Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ShoppingCartService shoppingCartService, ApplicationDbContext context)
        {
            _shoppingCartService = shoppingCartService;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.Identity.Name;
            var cart = _context.ShoppingCart
                               .Include(sc => sc.Items)
                               .ThenInclude(si => si.Product)
                               .FirstOrDefault(sc => sc.UserId == userId);

              // Assuming the user is authenticated and their username is used as UserId
            
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = User.Identity.Name;  // Assuming the user is authenticated and their username is used as UserId
            _shoppingCartService.AddItemToCart(userId, productId, 0);
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

        public decimal CalculateTotalPrice(string userId)
        {
            var cart = _context.ShoppingCart
                               .Include(sc => sc.Items)
                               .ThenInclude(si => si.Product)
                               .FirstOrDefault(sc => sc.UserId == userId);

            return cart.Items.Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}
