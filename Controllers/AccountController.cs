using HomeServices.Models;
using HomeServices.ViewModels; // Matches your root-level folder
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeServices.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager ,
                                 SignInManager<ApplicationUser> signInManager ,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email ,
                    Email = model.Email ,
                    FullName = model.FullName.Trim(), 
                    Address = model.Address ,
                    PhoneNumber = model.PhoneNumber ,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user , model.Password);

                if (!await _roleManager.RoleExistsAsync(model.UserRole))
                    await _roleManager.CreateAsync(new IdentityRole(model.UserRole));

                await _userManager.AddToRoleAsync(user, model.UserRole);
                await _signInManager.SignInAsync(user , isPersistent: false);
                    return RedirectToAction("Index" , "Home");
                
                foreach (var error in result.Errors) ModelState.AddModelError("" , error.Description);
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
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email , model.Password , model.RememberMe , lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index" , "Home");
                }

                ModelState.AddModelError(string.Empty , "Invalid login attempt. Check your email or password.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index" , "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRole = roles.FirstOrDefault();

            return View(user);
        }
    }
}