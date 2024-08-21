using eCommerceApp.Data;
using eCommerceApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public string HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hash = ComputeHash(password, salt);
            return $"{salt}.{hash}";
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
            {
                return false; // No hash stored, invalid
            }

            var parts = storedHash.Split('.');
            if (parts.Length != 2)
            {
                return false; // Invalid format
            }

            var salt = parts[0];
            var hash = parts[1];

            var computedHash = ComputeHash(password, salt);
            return hash == computedHash;
        }

        private string ComputeHash(string password, string salt)
        {
            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);
            return Convert.ToBase64String(hashBytes);
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
