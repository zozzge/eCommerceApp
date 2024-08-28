﻿using Microsoft.AspNetCore.Identity;
using eCommerceApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using eCommerceApp.ViewModels;

namespace eCommerceApp.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
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
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id.ToString());
            }
            return result;
        }

        // Sign out the current user
        public async Task SignOutAsync()
        {
            _httpContextAccessor.HttpContext.Session.Remove("UserId");
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        // Find a user by email
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        // Create a new user
        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        // Add user to a role
        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }
    }
}
