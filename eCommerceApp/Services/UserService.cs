using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private const int SaltSize = 16; // Size of the salt
        private const int HashSize = 32; // Size of the hash

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null && VerifyPasswordHash(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hash = Convert.FromBase64String(storedHash);
            using (var hmac = new HMACSHA256())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(computedHash);
            }
        }
    }
}
