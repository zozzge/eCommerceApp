using eCommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace eCommerceApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Product> Product { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ShoppingCart and ShoppingCartItem relationship
            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.Items)
                .WithOne(sci => sci.ShoppingCart)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

            // Configure ShoppingCartItem and Product relationship
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany() // Assuming Product doesn't need a navigation property back to ShoppingCartItem
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            // Note: If Product has a navigation property to ShoppingCartItems, configure it here as well
        }

    }


}
