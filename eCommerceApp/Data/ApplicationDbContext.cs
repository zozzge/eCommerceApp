using eCommerceApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace eCommerceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<PaymentOptions> PaymentOptions { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<ShoppingCartItem> ShoppingCartItem { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShoppingCart>()
            .HasMany(sc => sc.Items)
            .WithOne(si => si.ShoppingCart)
            .HasForeignKey(si => si.ShoppingCartId)
            .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(si => si.Product)
                .WithMany(p => p.ShoppingCartItems) // Ensure to configure reverse navigation in Product if needed
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            // Configure User entity if necessary
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }

    }


}
