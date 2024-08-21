using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;

        public AccountController(UserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.ValidateUserAsync(model.Email, model.Password);

                if (user != null)
                {
                    await SignInUserAsync(user);
                    return Redirect(returnUrl ?? Url.Action("Checkout", "Payment"));
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await _context.User.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email already in use.");
                    return View(model);
                }

                var user = new User
                {
                    Email = model.Email,
                    PasswordHash = _userService.HashPassword(model.Password) // Ensure secure password hashing
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                try
                {
                    await SignInUserAsync(user);
                    //await SignInUserAsync(user);
                    //return Redirect(returnUrl ?? Url.Action("Checkout", "Checkout"));
                }
                catch (Exception ex)
                {
                    // Log the exception or handle the error
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the account.");
                    return View(model);
                }

                // Automatically sign in the user after registration
                

                return Redirect(returnUrl ?? Url.Action("Checkout", "Checkout"));
            }

            return View(model);
        }

        private async Task SignInUserAsync(User user)
        {


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

