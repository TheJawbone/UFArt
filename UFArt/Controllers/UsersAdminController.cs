using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using UFArt.Infrastructure.Mailing;
using UFArt.Models.Identity;
using UFArt.Models.TextAssets;

namespace UFArt.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersAdminController : Controller
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IUserValidator<User> _userValidator;
        private IPasswordHasher<User> _passwordHasher;
        private IPasswordValidator<User> _passwordValidator;
        private IEmailService _emailService;
        private IEmailConfiguration _emailConfiguration;
        private ITextAssetsRepository _textRepository;
        private IHttpContextAccessor _contextAccessor;

        public UsersAdminController(UserManager<User> userManager, IUserValidator<User> userValidator,
            IPasswordHasher<User> passwordHasher, RoleManager<IdentityRole> roleManager, ITextAssetsRepository textRepository,
            IPasswordValidator<User> passwordValidator, IEmailService emailService, IEmailConfiguration emailConfiguration,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
            _textRepository = textRepository;
            _passwordValidator = passwordValidator;
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index(bool userCreated = false, bool userUpdated = false) =>
            View(new UsersManageViewModel(_userManager.Users, _textRepository)
            {
                UserCreated = userCreated,
                UserUpdated = userUpdated
            });

        [AllowAnonymous]
        public IActionResult CreateUser() => View(new UserCreateModel(_textRepository));

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserAsync(UserCreateModel model)
        {
            if (model.Password != model.PasswordConfirmation)
                ModelState.AddModelError("PasswordMismatchError", "Hasła muszą się zgadzać");

            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Name, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var validPassword = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                if (validPassword.Succeeded)
                {
                    IdentityResult createResult = await _userManager.CreateAsync(user, model.Password);
                    IdentityResult addToUsersResult = await _userManager.AddToRoleAsync(user, "user");

                    if (createResult.Succeeded && addToUsersResult.Succeeded)
                    {
                        var message = new EmailMessageFactory(_emailConfiguration).CreateActivationMessage(user, Request);
                        _emailService.Send(message);
                        if (_contextAccessor.HttpContext.User.IsInRole("admin"))
                            return RedirectToAction("Index", new { userCreated = true });
                        else
                        {
                            var queryParams = new Dictionary<string, string>();
                            queryParams["returnUri"] = "/About";
                            queryParams["messageKey"] = "success_confirmation_email_sent";
                            return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
                        }
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
            if (user != null) return View(new UserEditViewModel(_textRepository)
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber

            });
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditViewModel viewModel)
        {
            User user = await _userManager.FindByIdAsync(viewModel.Id);

            if (user != null)
            {
                user.Email = viewModel.Email;
                user.PhoneNumber = viewModel.PhoneNumber;
                user.UserName = viewModel.Username;
                user.EmailConfirmed = true;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded) AddErrorsFromResult(validEmail);

                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(viewModel.NewPassword))
                {
                    validPassword = await _passwordValidator.ValidateAsync(_userManager, user, viewModel.NewPassword);
                    if (validPassword.Succeeded) user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.NewPassword);
                    else AddErrorsFromResult(validPassword);
                }

                if ((validEmail.Succeeded && validPassword == null) || (validEmail.Succeeded && viewModel.NewPassword != string.Empty && validPassword.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole("admin"))
                            return RedirectToAction("Index", new { userUpdated = true });
                        else
                        {
                            var queryParams = new Dictionary<string, string>()
                        {
                            { "returnUri", "/UsersAdmin/Index" },
                            { "messageKey", "success_user_updated" }
                        };
                            return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
                        }
                    }
                    else AddErrorsFromResult(result);
                }
            }
            else ModelState.AddModelError("", "User Not Found");

            return View(new UserEditViewModel(_textRepository)
            {
                Id = viewModel.Id,
                Email = viewModel.Email,
                Username = viewModel.Username,
                PhoneNumber = viewModel.PhoneNumber
            });
        }
    }
}