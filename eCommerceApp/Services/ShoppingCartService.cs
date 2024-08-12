using AspNetCore;
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

        public ShoppingCart GetCartByUserId(string userId)
        {
            return _context.ShoppingCarts
                .Include(sc => sc.Items)
                .ThenInclude(sci => sci.ProductId)
                .FirstOrDefault(sc => sc.UserId == userId);
        }

        public void AddItemToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.UnitPrice = _context.Products.Find(productId).Price;
            }
            else
            {
                cart.Items.Add(new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = _context.Products.Find(productId).Price
                });
            }

            _context.SaveChanges();
        }

        public void RemoveItemFromCart(string userId, int cartItemId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    _context.SaveChanges();
                }
            }
        }

        public void UpdateItemQuantity(string userId, int cartItemId, int quantity)
        {
            var cart = GetCartByUserId(userId);
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
