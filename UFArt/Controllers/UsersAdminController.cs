using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.Identity;

namespace UFArt.Controllers
{
    public class UsersAdminController : Controller
    {
        private UserManager<User> _userManager;
        private IUserValidator<User> _userValidator;
        private IPasswordHasher<User> _passwordHasher;
        private IPasswordValidator<User> _passwordValidator;

        public UsersAdminController(UserManager<User> userManager, IUserValidator<User> userValidator, IPasswordHasher<User> passwordHasher, IPasswordValidator<User> passwordValidator)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        public IActionResult Index() => View(_userManager.Users);

        [AllowAnonymous]
        public IActionResult CreateUser() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserAsync(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Name, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) return View("Success", new string[] { "Pomyślnie dodano użytkownika", "/UsersAdmin/" });
                else
                {
                    var errors = result.Errors.GroupBy(e => e.Code).Select(g => g.First()); // for some reason email error was duplicated
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("CreateUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) return RedirectToAction("Index");
                else AddErrorsFromResult(result);
            }
            else ModelState.AddModelError("", "User Not Found");

            return View("Index", _userManager.Users);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> EditUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null) return View(user);
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string email, string password)
        {
            User user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded) AddErrorsFromResult(validEmail);

                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPassword = await _passwordValidator.ValidateAsync(_userManager, user, password);
                    if (validPassword.Succeeded) user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    else AddErrorsFromResult(validPassword);
                }

                if ((validEmail.Succeeded && validPassword == null) || (validEmail.Succeeded && password != string.Empty && validPassword.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) return View("Success", new string[] { "Pomyślnie zaktualizowano użytkownika", "/UsersAdmin/Index"});
                    else AddErrorsFromResult(result);
                }
            }
            else ModelState.AddModelError("", "User Not Found");

            return View(user);
        }
    }
}