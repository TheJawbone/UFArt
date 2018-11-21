using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.Identity;
using UFArt.Models.TextAssets;

namespace UFArt.Controllers
{
    public class AccountActivationController : Controller
    {
        private UserManager<User> _userManager;
        private ITextAssetsRepository _textRepo;

        public AccountActivationController(UserManager<User> userManager, ITextAssetsRepository textRepo)
        {
            _userManager = userManager;
            _textRepo = textRepo;
        }

        public async Task<IActionResult> Index(string code)
        {
            User user = await _userManager.FindByIdAsync(code);
            if (user != null)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return View("ActivationSuccess", new AccountActivationViewModel(_textRepo));
                else return View("Error");
            }
            else 
                return View("Error");
        }
    }
}