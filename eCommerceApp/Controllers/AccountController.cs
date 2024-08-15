using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace eCommerceApp.Controllers
{
    public class AccountController:Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.User.SingleOrDefault(u => u.Email == model.Email);
            if (user == null || !VerifyPasswordHash(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Set user ID in the session or a cookie
            HttpContext.Session.SetString("UserId", user.Id);

            return RedirectToLocal(returnUrl);
        }

        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.User.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email is already taken.");
                    return View(model);
                }

                var hashedPassword = HashPassword(model.Password);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    PasswordHash = hashedPassword
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Set user ID in the session or a cookie
                HttpContext.Session.SetString("UserId", user.Id);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }

        private bool VerifyPasswordHash(string password, string hashedPassword)
        {
            // Implement password hash verification logic here
            // This is a basic example using PBKDF2
            var parts = hashedPassword.Split(':');
            var salt = parts[0];
            var hash = parts[1];

            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000);
            var hashBytes = pbkdf2.GetBytes(20);

            return Convert.ToBase64String(hashBytes) == hash;
        }

        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            // This is a basic example using PBKDF2
            var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000);
            var hashBytes = pbkdf2.GetBytes(20);

            return $"{salt}:{Convert.ToBase64String(hashBytes)}";
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
