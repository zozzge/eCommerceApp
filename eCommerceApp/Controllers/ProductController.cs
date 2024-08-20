using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eCommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductController(ProductService productService, ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Product> products = (List<Product>)_productService.GetAllProducts();
            return View(products);
        }

       
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var cartIdCookie = Request.Cookies["CartId"];
            int cartId;
            var product = _context.Product.Find(productId);
            //int quantity1 = quantity + 1;
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] = "Invalid quantity.";
                return RedirectToAction("Index", "Home");
            }

            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index", "Home");
            }

            if (cartIdCookie != null && int.TryParse(cartIdCookie, out cartId))
            {
                cartId = int.Parse(cartIdCookie);
            }
            else
            {
                var newCart = new ShoppingCart
                {
                    UserId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null
                };
                _context.ShoppingCart.Add(newCart);
                _context.SaveChanges();
                cartId = newCart.Id;
                Response.Cookies.Append("CartId", cartId.ToString(), new CookieOptions { HttpOnly = true, Expires = DateTimeOffset.Now.AddDays(30) });
            }

            var shoppingCart = _context.ShoppingCart
                .Include(sc => sc.Items) // Ensure Items are included in the query
                .FirstOrDefault(sc => sc.Id == cartId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    Id = cartId,
                    UserId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null
                };
                _context.ShoppingCart.Add(shoppingCart);
                _context.SaveChanges();
            }

            var existingItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == productId);


            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    ProductId = product.Id,
                    Product = product,
                    ShoppingCartId = shoppingCart.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                shoppingCart.Items.Add(newItem);
                
            }

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cartId = Request.Cookies["CartId"] ?? Guid.NewGuid().ToString();
            var shoppingCart = _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefault(sc => sc.Id.ToString() == cartId);


            if (shoppingCart == null)
            {
                // Handle the case where the shopping cart does not exist
                TempData["ErrorMessage"] = "Shopping cart not found.";
                return RedirectToAction("Index", "Home");
            }

            // Find the item to remove
            var itemToRemove = shoppingCart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (itemToRemove != null)
            {
                // Remove the item from the shopping cart's items list
                shoppingCart.Items.Remove(itemToRemove);

                // Remove the item from the ShoppingCartItems table
                _context.ShoppingCartItem.Remove(itemToRemove);

                // Save changes to the database
                _context.SaveChanges();
            }
            else
            {
                // Handle the case where the item is not found in the cart
                TempData["ErrorMessage"] = "Item not found in cart.";
            }

            return RedirectToAction("Index", "Home"); // Redirect to shopping cart page or any other relevant page
        }

    }
}
