using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace eCommerceApp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            // Verify the password
            if (VerifyPasswordHash(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Example hashing function. Ensure this matches your storage method.
            // This example assumes you are using a method like PBKDF2
            var hash = Convert.FromBase64String(storedHash);
            using (var hmac = new HMACSHA256())
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(computedHash);
            }
        }
    }
}
