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

            if (cart == null)
            {
                return View("EmptyCart");
            }

            // Calculate total price
            ViewBag.TotalPrice = cart.Items.Sum(item => item.UnitPrice * item.Quantity);

            return View(cart);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cart = _context.ShoppingCart
                               .Include(sc => sc.Items)
                               .FirstOrDefault(sc => sc.UserId == User.Identity.Name);

            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    _context.ShoppingCartItem.Remove(item);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateItemQuantity(int cartItemId, int quantity)
        {
            var cart = _context.ShoppingCart
                               .Include(sc => sc.Items)
                               .FirstOrDefault(sc => sc.UserId == User.Identity.Name);

            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (item != null)
                {
                    if (quantity <= 0)
                    {
                        // Remove item if quantity is 0 or less
                        _context.ShoppingCartItem.Remove(item);
                        cart.Items.Remove(item);
                    }
                    else
                    {
                        // Update quantity
                        item.Quantity = quantity;
                    }

                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.Name;

            // Retrieve the shopping cart for the user
            var shoppingCart = _context.ShoppingCart
                .Include(sc => sc.Items)
                .ThenInclude(si => si.Product)
                .FirstOrDefault(sc => sc.UserId == userId);

            if (shoppingCart == null || !shoppingCart.Items.Any())
            {
                // Handle the case where the cart is empty or doesn't exist
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index", "Products");
            }

            var totalPrice = shoppingCart.Items.Sum(item => item.UnitPrice * item.Quantity);
            ViewBag.TotalPrice = totalPrice;
            // Process the payment or display the checkout view
            return View(shoppingCart); // Ensure you have a corresponding view for Checkout
        }


    }
}
