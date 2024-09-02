﻿using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using eCommerceApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceApp.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IPaymentService _paymentService;

        public CheckOutController(ApplicationDbContext context, UserService userService, ShoppingCartService shoppingCartService, IPaymentService paymentService)
        {
            _context = context;
            _userService = userService;
            _shoppingCartService = shoppingCartService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var paymentOptions = await _paymentService.GetPaymentOptionsAsync();

            // Retrieve the total price from TempData
            if (TempData.ContainsKey("TotalPrice"))
            {
                if (TempData["TotalPrice"] is string decimalString)
                {
                    // Parse the string to decimal
                    if (Decimal.TryParse(decimalString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal totalPrice))
                    {
                        ViewBag.TotalPrice = totalPrice;
                    }
                    else
                    {
                        // Handle the case where parsing fails
                        ViewBag.TotalPrice = 0.0m; // or some default value
                    }
                }
            }

            return View(paymentOptions);

        }

        public async Task<IActionResult> CheckOut()
        {
            var paymentOptions = await _context.PaymentOptions.ToListAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            

            // Handle TempData for TotalPrice
            if (TempData.ContainsKey("TotalPrice"))
            {
                string decimalString = TempData["TotalPrice"].ToString();
                decimal totalPrice = Decimal.Parse(decimalString, CultureInfo.InvariantCulture);

                ViewBag.TotalPrice = totalPrice; // Use 'totalPrice' variable instead of TempData directly
            }

            if (userId == null)
            {
                // Handle anonymous cart logic
                var anonymousCartId = Request.Cookies["CartId"];
                if (anonymousCartId != null)
                {
                    var anonymousCart = await GetShoppingCartAsync(anonymousCartId);

                    if (anonymousCart != null)
                    {
                        // Store the anonymous cart ID temporarily
                        Response.Cookies.Append("TempCartId", anonymousCartId);
                        // Redirect to Login if needed
                        return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Checkout") });
                    }
                }
                else
                {
                    // Redirect to Login page with returnUrl parameter
                    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Checkout") });
                }
            }
            else
            {
                var userCart = await _shoppingCartService.GetCartByUserIdAsync();

                //?????
                if (userCart != null)
                {
                    return View("CheckOut", new { PaymentOptions = paymentOptions, TotalPrice = ViewBag.TotalPrice });
                }
                else
                {
                    // No cart found for the user
                    return RedirectToAction("Index", "Home");
                }

            }

            return RedirectToAction("Index", "Home");
        }

        private async Task<ShoppingCart> GetShoppingCartAsync(string cartId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.Id.ToString() == cartId);
        }

        private async Task HandleAuthenticatedUserAsync(string userId, ShoppingCart anonymousCart)
        {
            var userCart = await _shoppingCartService.GetCartByUserIdAsync();

            if (userCart == null)
            {
                // Create a new shopping cart for the user if none exists
                userCart = new ShoppingCart
                {
                    UserId = userId,
                    Items = new List<ShoppingCartItem>()
                };
                _context.ShoppingCarts.Add(userCart);
                await _context.SaveChangesAsync();
            }

            // Merge anonymous cart items with user cart
            foreach (var item in anonymousCart.Items)
            {
                var existingItem = userCart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    var newItem = new ShoppingCartItem
                    {
                        ProductId = item.ProductId,
                        Product = item.Product,
                        ShoppingCartId = userCart.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice // Ensure to include the price if needed
                    };
                    userCart.Items.Add(newItem);
                }
            }

            await _context.SaveChangesAsync();
        }

        
    }
}
