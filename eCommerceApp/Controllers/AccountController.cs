using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserService _userService;

        public AccountController(UserService userService)
        {

            _userService = userService;
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
                var result = await _userService.SignInAsync(model);

                if (result.Succeeded)
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Checkout"));
                }
                else if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
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
                var result = await _userService.RegisterAsync(model);

                if (result.Succeeded)
                {
                    var loginModel = new LoginViewModel
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    await _userService.SignInAsync(loginModel);
                    return Redirect(returnUrl ?? Url.Action("Login", "Account"));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
