namespace eCommerceApp.Models
{
    public class ShoppingCartViewModel
    {
        public int CartId { get; set; }
        public List<ShoppingCartItemViewModel> Items { get; set; } = new List<ShoppingCartItemViewModel>();
    }
}
