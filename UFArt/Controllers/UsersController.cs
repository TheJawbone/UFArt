using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure;
using UFArt.Models.Identity;

namespace UFArt.Controllers
{
    [Authorize(Roles = "user")]
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signinMgr)
        {
            _userManager = userManager;
            _signInManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    if (user.EmailConfirmed == false) return View("AccountInactive");
                    else
                    {
                        await _signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result =
                            await _signInManager.PasswordSignInAsync(user, details.Password, true, false);
                        if (result.Succeeded) return Redirect(returnUrl ?? "/About");
                    }
                }

                ModelState.AddModelError(nameof(UserLoginModel.Email), "Invalid user or password");
            }

            return View(details);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/About");
        }
    }
}