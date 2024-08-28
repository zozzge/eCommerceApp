using eCommerceApp.Data;
using eCommerceApp.Models;
using eCommerceApp.Services;
using eCommerceApp.ViewModels;
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
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;



        public AccountController(UserService userService, ApplicationDbContext context,SignInManager<User> signInManager)
        {

            _userService = userService;
            _context = context;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.FindByEmailAsync(model.Email);

            if (user != null)
            {
                //User is found, check password
                var passwordCheck = await _userService.CheckPasswordAsync(user, model.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Checkout");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(model);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.FindByEmailAsync(model.Email);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(model);
            }

            var newUser = new User()
            {
                Email = model.Email,
                UserName = model.Email
            };

            var newUserResponse = await _userService.CreateAsync(newUser, model.Password);

            if (newUserResponse.Succeeded)
            {
                TempData["Success"] = "Registration successful. Please log in.";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in newUserResponse.Errors)
            {
                TempData["Error"] = error.Description;
            }

            return View(model);




        }

        [HttpPost]

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
