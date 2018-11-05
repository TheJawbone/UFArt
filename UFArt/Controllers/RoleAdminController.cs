using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.Identity;

namespace UFArt.Controllers
{
    //[Authorize(Roles = "admin")]
    public class RoleAdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ViewResult Index() => View(_roleManager.Roles);

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required]string roleName)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded) return RedirectToAction("Index");
                else AddErrorsFromResult(result);
            }

            return View(roleName);
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<User> members = new List<User>();
            List<User> nonMembers = new List<User>();

            foreach (User user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    User user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded) AddErrorsFromResult(result);
                    }
                }

                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    User user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded) AddErrorsFromResult(result);
                    }
                }
            }

            if (ModelState.IsValid) return View("Success", new string[] { "Pomyślnie zmodyfikowano rolę", "/RoleAdmin"});
            else return await Edit(model.RoleId);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded) return RedirectToAction("Index");
                else AddErrorsFromResult(result);
            }
            else ModelState.AddModelError("", "No role found");

            return View("Index", _roleManager.Roles);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}