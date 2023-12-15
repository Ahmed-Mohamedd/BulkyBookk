using BulkyBook.DAL.Entities;
using BulkyBook.PL.Utilities;
using BulkyBook.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.PL.Areas.Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _roleManager=roleManager;
        }

        public IActionResult  Register()
        {
            if(!_roleManager.RoleExistsAsync(Roles.AdminRole).GetAwaiter().GetResult())
            {
            _roleManager.CreateAsync(new IdentityRole(Roles.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.CustomerRole)).GetAwaiter().GetResult();
            }

            IEnumerable<SelectListItem> RolesList = _roleManager.Roles.Select(c =>
           new SelectListItem()
           {
               Text = c.Name,
               Value = c.Name
           });
            ViewBag.RoleList = RolesList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {



            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName=registerViewModel.Email.Split("@")[0],
                    Email=registerViewModel.Email,
                    State = registerViewModel.State,
                    City = registerViewModel.City,
                    StreetName = registerViewModel.StreetAddress,
                    PostalCode = registerViewModel.PostalCode,
                    IsAgree= registerViewModel.IsAgree
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    if (registerViewModel.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.CustomerRole);
                    }
                    else 
                    {
                        await _userManager.AddToRoleAsync(user, registerViewModel.Role);
                    }
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    var CheckPasswordCorrectence = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                    if (CheckPasswordCorrectence)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home", new { area = "Customer"} );
                    }
                    ModelState.AddModelError(string.Empty, "Password Is Incorrect");
                }
                ModelState.AddModelError(string.Empty, "User Not Found");
            }
            return View(loginViewModel);
            //    return RedirectToAction(nameof(Areas.Admin.Controllers.ProductController.Index));

            //    return RedirectToAction(nameof(Areas.ControlPanel.Controllers.HomeController.Index), nameof(Areas.ControlPanel.Controllers.HomeController).Replace("Controller", ""), new { area = nameof(Areas.ControlPanel) });
            //}
        }

        public  async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login), "Account" , new {area = "Identity"});
        }


    }
}
