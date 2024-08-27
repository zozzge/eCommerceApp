using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentOptions>> GetPaymentOptionsAsync()
        {
            return await _context.PaymentOptions.ToListAsync();
        }

        public async Task<decimal?> GetTotalPriceAsync()
        {
            var cartItems = await _context.ShoppingCartItems.ToListAsync();
            decimal? cartItemsSum = cartItems.Sum(item => item.UnitPrice * item.Quantity);
            return cartItemsSum;
        }
    }
}