namespace eCommerceApp.ViewModels
{
    public class PaymentOptionsViewModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal? TotalPrice { get; set; }
        public IEnumerable<Models.PaymentOptions> PaymentOptions { get; set; }
    }
}
