using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceApp.Services
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get the user's cart based on user ID
        public async Task<ShoppingCart> GetCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCart
                                 .Include(sc => sc.Items)
                                 .ThenInclude(si => si.Product)
                                 .FirstOrDefaultAsync(sc => sc.UserId == userId);
        }

        // Add an item to the cart
        public async Task AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var shoppingCart = await GetCartByUserIdAsync(userId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    UserId = userId,
                    Items = new List<ShoppingCartItem>()
                };
                _context.ShoppingCart.Add(shoppingCart);
            }

            var product = await _context.Product.FindAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
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
                    ProductId = productId,
                    Product = product,
                    ShoppingCartId = shoppingCart.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price // Assuming you want to store unit price
                };
                shoppingCart.Items.Add(newItem);
            }

            await _context.SaveChangesAsync();
        }

        // Remove an item from the cart
        public async Task RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            var cart = await GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                throw new KeyNotFoundException("Shopping cart not found");
            }

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (item != null)
            {
                _context.ShoppingCartItem.Remove(item); // Remove from DbContext
                cart.Items.Remove(item); // Remove from list
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Cart item not found");
            }
        }

        // Update item quantity in the cart
        public async Task UpdateItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                throw new KeyNotFoundException("Shopping cart not found");
            }

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (item != null)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Cart item not found");
            }
        }
    }
}
