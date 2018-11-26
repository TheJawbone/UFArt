using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure;
using UFArt.Models.Identity;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    [Authorize(Roles = "user")]
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ITextAssetsRepository _textRepo;
        private IHttpContextAccessor _contextAccessor;
        private IPasswordHasher<User> _passwordHasher;
        private IPasswordValidator<User> _passwordValidator;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager,
            ITextAssetsRepository textRepository, IHttpContextAccessor contextAccessor,
            IPasswordHasher<User> passwordHasher, IPasswordValidator<User> passwordValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _textRepo = textRepository;
            _contextAccessor = contextAccessor;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        public async Task<IActionResult> AccountOverview()
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            return View(new UserViewModel(_textRepo, user));
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new UserLoginModel(_textRepo));
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
                    if (user.EmailConfirmed == false) return View("AccountInactive", new ViewModel(_textRepo));
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

            details.TextRepository = _textRepo;
            return View(details);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/About");
        }

        public async Task<IActionResult> ChangeAccountData()
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            return View(new ChangeAccountDataViewModel(_textRepo, user));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAccountData(ChangeAccountDataViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id);
            if (user != null)
            {
                try
                {
                    user.UserName = viewModel.Username;
                    user.PhoneNumber = viewModel.PhoneNumber;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("AccountOverview");
                }
                catch (Exception ex) { return View("Error"); }
            }
            else return View("Error");
        }

        public IActionResult ChangePassword(ChangePasswordViewModel viewModel = null)
        {
            if (viewModel.Id == null)
            {
                ModelState.Clear();
                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                viewModel = new ChangePasswordViewModel(_textRepo, userId);
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordPost(ChangePasswordViewModel viewModel)
        {
            if (viewModel.NewPassword != viewModel.NewPasswordConfirmed)
                ModelState.AddModelError("PasswordMismatchError", _textRepo.GetTranslatedValue("passwords_must_match", Request.HttpContext));

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(viewModel.Id);
                if (user != null)
                {
                    var validPassword = await _passwordValidator.ValidateAsync(_userManager, user, viewModel.NewPassword);
                    if (validPassword.Succeeded)
                    {
                        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, viewModel.OldPassword);
                        if (result == PasswordVerificationResult.Success)
                        {
                            user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.NewPassword);
                            await _userManager.UpdateAsync(user);
                            await _signInManager.SignOutAsync();
                            return Redirect("/About");
                        }
                        else
                        {
                            ModelState.AddModelError("InvalidPasswordError", _textRepo.GetTranslatedValue("invalid_old_password", Request.HttpContext));
                            viewModel.TextRepository = _textRepo;
                            return View("ChangePassword", viewModel);
                        }
                    }
                    else
                    {
                        foreach (var error in validPassword.Errors)
                        {
                            ModelState.AddModelError("PasswordError", error.Description);
                        }
                        viewModel.TextRepository = _textRepo;
                        return View("ChangePassword", viewModel);
                    }
                }
                else return View("Error");
            }
            else
            {
                viewModel.TextRepository = _textRepo;
                return View("ChangePassword", viewModel);
            }
        }
    }
}