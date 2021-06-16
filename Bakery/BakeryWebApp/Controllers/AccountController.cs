using BakeryWebApp.Models;
using BakeryWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_userManager.Users.Count() == 0)
            {
                User admin = new User { Email = "admin", UserName = "admin", Year = 2000 };
                User manager = new User { Email = "manager", UserName = "manager", Year = 2000 };
                User user = new User { Email = "user", UserName = "user", Year = 2000 };
                await _userManager.CreateAsync(admin, "admin");
                await _userManager.CreateAsync(manager, "manager");
                await _userManager.CreateAsync(user, "user");
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("manager"));
                await _roleManager.CreateAsync(new IdentityRole("user"));
                List<string> role = new List<string>();
                role.Add("admin");
                await _userManager.AddToRolesAsync(admin, role);
                role = new List<string>();
                role.Add("manager");
                await _userManager.AddToRolesAsync(manager, role);
                role = new List<string>();
                role.Add("user");
                await _userManager.AddToRolesAsync(user, role);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                var result = await _userManager.CreateAsync(user, model.Password);
                List<string>role = new List<string>();
                role.Add("user");
                await _userManager.AddToRolesAsync(user, role);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (_userManager.Users.Count() == 0)
            {
                User admin = new User { Email = "admin", UserName = "admin", Year = 2000 };
                User manager = new User { Email = "manager", UserName = "manager", Year = 2000 };
                User user = new User { Email = "user", UserName = "user", Year = 2000 };
                await _userManager.CreateAsync(admin, "admin");
                await _userManager.CreateAsync(manager, "manager");
                await _userManager.CreateAsync(user, "user");
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("manager"));
                await _roleManager.CreateAsync(new IdentityRole("user"));
                List<string> role = new List<string>();
                role.Add("admin");
                await _userManager.AddToRolesAsync(admin, role);
                role = new List<string>();
                role.Add("manager");
                await _userManager.AddToRolesAsync(manager, role);
                role = new List<string>();
                role.Add("user");
                await _userManager.AddToRolesAsync(user, role);
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
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
