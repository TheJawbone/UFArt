using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure.Mailing;
using UFArt.Models.Identity;

namespace UFArt.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersAdminController : Controller
    {
        private UserManager<User> _userManager;
        private IUserValidator<User> _userValidator;
        private IPasswordHasher<User> _passwordHasher;
        private RoleManager<IdentityRole> _roleManager;
        private IPasswordValidator<User> _passwordValidator;
        private IEmailService _emailService;
        private IEmailConfiguration _emailConfiguration;

        public UsersAdminController(UserManager<User> userManager, IUserValidator<User> userValidator,
            IPasswordHasher<User> passwordHasher, RoleManager<IdentityRole> roleManager,
            IPasswordValidator<User> passwordValidator, IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
            _passwordValidator = passwordValidator;
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }

        public IActionResult Index() => View(_userManager.Users);

        [AllowAnonymous]
        public IActionResult CreateUser() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserAsync(UserCreateModel model)
        {
            if(model.Password != model.PasswordConfirmation)
                ModelState.AddModelError("PasswordMismatchError", "Hasła muszą się zgadzać");

            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Name, Email = model.Email };
                var validPassword = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                if (validPassword.Succeeded)
                {
                    IdentityResult createResult = await _userManager.CreateAsync(user, model.Password);
                    IdentityResult addToUsersResult = await _userManager.AddToRoleAsync(user, "user");

                    if (createResult.Succeeded && addToUsersResult.Succeeded)
                    {
                        var message = new EmailMessageFactory(_emailConfiguration).CreateActivationMessage(user, Request);
                        _emailService.Send(message);
                        return View("Success", new string[] { "Pomyślnie dodano użytkownika", "/UsersAdmin/" });
                    }
                    else
                    {
                        var errors = createResult.Errors.GroupBy(e => e.Code).Select(g => g.First()); // for some reason email error was duplicated
                        foreach (var error in errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in validPassword.Errors)
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