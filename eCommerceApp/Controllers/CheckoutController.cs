using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            // Check if the user is authenticated
            var userId = User.Identity.IsAuthenticated ? User.Identity.Name : null;

            if (!User.Identity.IsAuthenticated)
            {
                // If not authenticated, handle the anonymous cart logic
                var anonymousCartId = Request.Cookies["CartId"];
                if (anonymousCartId != null)
                {
                    // Retrieve the anonymous cart
                    var anonymousCart = _context.ShoppingCart
                        .Include(sc => sc.Items)
                        .FirstOrDefault(sc => sc.Id.ToString() == anonymousCartId);

                    if (anonymousCart != null)
                    {
                        // If user is not authenticated, create a temporary user cart
                        if (userId == null)
                        {
                            // Create a temporary cart to hold items until the user logs in
                            Response.Cookies.Append("TempCartId", anonymousCartId);
                        }
                        else
                        {
                            // Handle the authenticated user scenario
                            var userCart = _context.ShoppingCart
                                .Include(sc => sc.Items)
                                .FirstOrDefault(sc => sc.UserId == userId);

                            if (userCart == null)
                            {
                                userCart = new ShoppingCart
                                {
                                    UserId = userId,
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
                                        Quantity = item.Quantity
                                    };
                                    userCart.Items.Add(newItem);
                                }
                            }

                            // Remove the anonymous cart and delete the cookie
                            _context.ShoppingCart.Remove(anonymousCart);
                            Response.Cookies.Delete("CartId");

                            _context.SaveChanges();

                            // Redirect to checkout confirmation page
                            return RedirectToAction("CheckoutConfirmation", "Order");
                        }
                    }
                }
                else
                {
                    // If there is no anonymous cart and user is not authenticated
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                // Handle the case when user is authenticated
                var userCart = _context.ShoppingCart
                    .Include(sc => sc.Items)
                    .FirstOrDefault(sc => sc.UserId == userId);

                if (userCart != null)
                {
                    // Redirect to checkout confirmation page
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
    }
}
