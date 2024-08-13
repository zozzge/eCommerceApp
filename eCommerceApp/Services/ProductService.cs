using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.Data.SqlClient;

namespace eCommerceApp.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                return _context.Product.ToList();
            }
            catch (SqlException ex)
            {
                // Log the exception details
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Product GetProductById(int productId)
        {
            return _context.Product.Find(productId);
        }
    }
}
