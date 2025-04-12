using CoreMart.DAL.Models;
using CoreMart.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreMart.PL.Controllers
{
    public class AccountController : Controller
    {

            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AccountController(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager,
                                     RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _roleManager = roleManager;
            }

            // Register GET
            public IActionResult Register() => View();

            // Register POST
            [HttpPost]
            public async Task<IActionResult> Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // Assign default role
                        await _userManager.AddToRoleAsync(user, "Customer");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }

                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            // ===== Login =====
            public IActionResult Login(string? returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl;

                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }

                    ModelState.AddModelError("", "Invalid login attempt.");
                }

                return View(model);
            }

            // ===== Logout =====
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            // ===== Create Roles (once only) =====
            public async Task<IActionResult> CreateRoles()
            {
                string[] roles = { "Admin", "Customer" };

                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        await _roleManager.CreateAsync(new IdentityRole(role));
                }

                return Content("Roles created successfully");
            }

            // ===== Assign Role to user manually =====
            public async Task<IActionResult> MakeAdmin(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    return Content($"{email} is now Admin");
                }

                return NotFound();
            }
        }
    }

