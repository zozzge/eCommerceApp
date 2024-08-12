namespace eCommerceApp.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public Product Product { get; set; }
        public ShoppingCartItem ShoppingCartItem { get; set; }
    }
}
