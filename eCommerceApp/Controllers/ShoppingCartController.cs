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
            ViewBag.TotalPrice = cart.Items.Sum(item => (item.UnitPrice ?? 0) * (item.Quantity ?? 0));

            return View(cart);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var item = _context.ShoppingCartItem
                       .Include(si => si.ShoppingCart)
                       .FirstOrDefault(si => si.Id == cartItemId);

            if (item != null)
            {
                var cart = item.ShoppingCart;
                cart.Items.Remove(item);
                _context.ShoppingCartItem.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateItemQuantity(int cartItemId, int quantity)
        {
            var item = _context.ShoppingCartItem
                       .Include(si => si.ShoppingCart)
                       .FirstOrDefault(si => si.Id == cartItemId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    _context.ShoppingCartItem.Remove(item);
                    item.ShoppingCart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        

        }


    }

