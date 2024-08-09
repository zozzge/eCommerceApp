using eCommerceApp.Data;
using eCommerceApp.Models;

namespace eCommerceApp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(string userId)
        {
            return _context.Users.Find(userId);
        }
    }
}
