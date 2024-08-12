using eCommerceApp.Data;
using eCommerceApp.Models;

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
            return _context.Product.ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Product.Find(productId);
        }
    }
}
