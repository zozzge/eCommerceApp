using Microsoft.AspNetCore.Identity;
using eCommerceApp.Models;
using System.Threading.Tasks;

namespace eCommerceApp.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register a new user
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        // Sign in an existing user
        public async Task<SignInResult> SignInAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
        }

        // Sign out the current user
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
