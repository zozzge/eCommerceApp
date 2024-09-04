using eCommerceApp.Models;

namespace eCommerceApp.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentOptions>> GetPaymentOptionsAsync();
        Task<decimal?> GetTotalPriceAsync(int cartId);
    }
}
