using eCommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Controllers
{
    public class UserController:Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }
    }
}
