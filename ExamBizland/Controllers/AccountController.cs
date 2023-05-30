using ExamBizland.Models;
using ExamBizland.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamBizland.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _user;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> user, SignInManager<AppUser> signInManager)
        {
            _user = user;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) { return View(); }
            AppUser user = new AppUser()
            {
                UserName = registerVM.UserName,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email
            };
            var result= await _user.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) { return View(); }
            if (loginVM.UserNameOrEmail.Contains("@"))
            {
                var user = await _user.FindByEmailAsync(loginVM.UserNameOrEmail);
                if(user == null)
                {
                    NotFound();
                    return View();
                }
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, true);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error");
                    return View();
                }
            }
            else
            {
                var user = await _user.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    NotFound();
                    return View();
                }
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, true);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error");
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
