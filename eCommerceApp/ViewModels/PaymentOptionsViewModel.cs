using System.Collections;
using System.Collections.Generic;
using eCommerceApp.Models;
namespace eCommerceApp.ViewModels
{
    public class PaymentOptionsViewModel
    {
        public decimal? TotalPrice { get; set; }
        public IEnumerable<PaymentOptions> PaymentOptions { get; set; }
    }
}
