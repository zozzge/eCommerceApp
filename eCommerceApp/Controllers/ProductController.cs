using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user is not logged in
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the shopping cart for the user
            var shoppingCart = _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefault(sc => sc.UserId == userId);

            if (shoppingCart == null)
            {
                // Create a new shopping cart if it doesn't exist
                shoppingCart = new ShoppingCart
                {
                    UserId = userId,
                    Items = new List<ShoppingCartItem>()
                };
                _context.ShoppingCart.Add(shoppingCart);
            }

            // Retrieve the product
            var product = _context.Product.Find(productId);

            if (product == null)
            {
                // Handle the case where the product does not exist
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index", "Products");
            }

            // Check if the product already exists in the shopping cart
            var existingItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Update quantity if the item already exists
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add a new item if it doesn't exist
                var newItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Product = product,
                    ShoppingCartId = shoppingCart.Id,
                    Quantity = quantity
                };

                shoppingCart.Items.Add(newItem);
            }

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("Index", "Products"); // Redirect to products page or any other relevant page
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user is not logged in
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the shopping cart for the user
            var shoppingCart = _context.ShoppingCart
                .Include(sc => sc.Items)
                .FirstOrDefault(sc => sc.UserId == userId);

            if (shoppingCart == null)
            {
                // Handle the case where the shopping cart does not exist
                TempData["ErrorMessage"] = "Shopping cart not found.";
                return RedirectToAction("Index", "Products");
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

            return RedirectToAction("Index", "ShoppingCart"); // Redirect to shopping cart page or any other relevant page
        }

    }
}
