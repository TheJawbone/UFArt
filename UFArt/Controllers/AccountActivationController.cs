using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.Identity;

namespace UFArt.Controllers
{
    public class AccountActivationController : Controller
    {
        private UserManager<User> _userManager;

        public AccountActivationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string code)
        {
            User user = await _userManager.FindByIdAsync(code);
            if (user != null)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return View("ActivationSuccess");
                else return View("Error");
            }
            else 
                return View("Error");
        }
    }
}