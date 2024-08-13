﻿namespace eCommerceApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        public List<ShoppingCartItem> ShoppingCartItem { get; set; }

        public int QuantityInCart { get; set; }
    }
}
