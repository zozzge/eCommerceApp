using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService; // Ensure you have a service for user operations

        public CheckoutController(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult CheckOut()
        {
            return View();
        }
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            if (!User.Identity.IsAuthenticated)
            {
                // Handle anonymous cart logic
                var anonymousCartId = Request.Cookies["CartId"];
                if (anonymousCartId != null)
                {
                    var anonymousCart = await GetShoppingCartAsync(anonymousCartId);

                    if (anonymousCart != null)
                    {
                        if (userId == null)
                        {
                            // Store the anonymous cart ID temporarily
                            Response.Cookies.Append("TempCartId", anonymousCartId);
                        }
                        else
                        {
                            await HandleAuthenticatedUserAsync(userId, anonymousCart);
                            // Remove the anonymous cart and delete the cookie
                            _context.ShoppingCart.Remove(anonymousCart);
                            Response.Cookies.Delete("CartId");
                            await _context.SaveChangesAsync();
                            return RedirectToAction("CheckoutConfirmation", "Order");
                        }
                    }
                }
                else
                {
                    // Redirect to Login page with returnUrl parameter
                    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Payment") });
                }
            }
            else
            {
                var userCart = await GetUserCartAsync(userId);
                if (userCart != null)
                {
                    return RedirectToAction("CheckoutConfirmation", "Order");
                }
                else
                {
                    // No cart found for the user
                    return RedirectToAction("Index", "Products");
                }
            }

            // In case no condition is met
            return RedirectToAction("Index", "Products");
        }

        private async Task<ShoppingCart> GetShoppingCartAsync(string cartId)
        {
            return await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.Id.ToString() == cartId);
        }

        private async Task<ShoppingCart> GetUserCartAsync(string userId)
        {
            return await _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);
        }

        private async Task HandleAuthenticatedUserAsync(string userId, ShoppingCart anonymousCart)
        {
            var userCart = await GetUserCartAsync(userId);

            if (userCart == null)
            {
                // Register the user if they don't exist
                var user = await RegisterUserAsync(userId);
                userCart = new ShoppingCart
                {
                    UserId = user.Id.ToString(),
                    Items = new List<ShoppingCartItem>()
                };
                _context.ShoppingCart.Add(userCart);
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
        }

        private async Task<User> RegisterUserAsync(string email)
        {
            var user = new User
            {
                Email = email,
                // Set other user properties as needed
                PasswordHash = "DefaultPassword" // Set a default or prompt for password
            };

            // Save the new user to the database
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
