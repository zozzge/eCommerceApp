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
        public IEnumerable<object> ShoppingCartItems { get; internal set; }

        public DbSet<PaymentOptions> PaymentOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            // Configure ShoppingCart and ShoppingCartItem relationship
            modelBuilder.Entity<ShoppingCart>()
            .HasMany(sc => sc.Items)
            .WithOne(si => si.ShoppingCart)
            .HasForeignKey(si => si.ShoppingCartId);

            modelBuilder.Entity<ShoppingCartItem>()
            .HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            ;
        }

    }


}
