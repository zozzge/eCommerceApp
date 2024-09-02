namespace eCommerceApp.ViewModels
{
    public class PaymentOptionsViewModel
    {
        public decimal? TotalPrice { get; set; }
        public IEnumerable<Models.PaymentOptions> PaymentOptions { get; set; }
    }
}
