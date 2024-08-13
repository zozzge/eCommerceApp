//using AspNetCore;
using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Services
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public ShoppingCart GetCartByUserId(string userId)
        //{
        //    var shoppingCart = _context.ShoppingCart
        //             .Include(sc => sc.Items) 
        //             .ThenInclude(si => si.Product) 
        //             .FirstOrDefault(sc => sc.UserId == userId);

        //    return shoppingCart;
        //}

        public void AddItemToCart(string userId, int productId, int quantity)
        {
            var shoppingCart = _context.ShoppingCart
                               .Include(sc => sc.Items)
                               .ThenInclude(si => si.Product)
                               .FirstOrDefault(sc => sc.UserId == userId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart { UserId = userId, Items = new List<ShoppingCartItem>() };
                _context.ShoppingCart.Add(shoppingCart);
            }

            // Retrieve the product
            var product = _context.Product.Find(productId);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Check if the product already exists in the shopping cart
            var existingItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // Update quantity if item already exists
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add new item if not exists
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
        }


        public void RemoveItemFromCart(string userId, int cartItemId)
        {
            var cart = _context.ShoppingCart
                      .Include(sc => sc.Items)
                      .FirstOrDefault(sc => sc.UserId == userId);
            //var cartItem = _context.ShoppingCartItem

            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId); // Ensure 'Id' matches your property

                if (item != null)
                {
                   /* _context.ShoppingCartItems.Remove(item); */// Remove from DbContext
                    cart.Items.Remove(item); // Remove from list
                    
                    _context.SaveChanges();
                }
            }
        }

        public void UpdateItemQuantity(string userId, int cartItemId, int quantity)
        {
            var cart = _context.ShoppingCart
                               .Include(sc => sc.Items)
                               .ThenInclude(si => si.Product)
                               .FirstOrDefault(sc => sc.UserId == userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (item != null)
                {
                    item.Quantity = quantity;
                    _context.SaveChanges();
                }
            }
        }

       


    }
}
