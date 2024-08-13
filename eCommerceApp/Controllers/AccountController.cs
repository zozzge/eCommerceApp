using eCommerceApp.Models;
using eCommerceApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Controllers
{
    public class AccountController:Controller
    {
        private readonly RegisterService _registerService;
        private readonly LoginService _loginService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(RegisterService registerService, LoginService loginService, SignInManager<IdentityUser> signInManager)
        {
            _registerService = registerService;
            _loginService = loginService;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _registerService.RegisterUserAsync(model);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(new IdentityUser { UserName = model.Email, Email = model.Email }, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _loginService.LoginUserAsync(model.Email, model.Password, model.RememberMe);

                if (success)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
