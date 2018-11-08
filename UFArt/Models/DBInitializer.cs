using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Identity;
using UFArt.Models.Newsfeed;

namespace UFArt.Models
{
    public static class DBInitializer
    {
        public static void ClearDatabase(IApplicationBuilder app)
        {
            ApplicationDbContext context = (ApplicationDbContext)app
                .ApplicationServices.GetService(typeof(ApplicationDbContext));
            context.Database.Migrate();
            context.Database.EnsureDeleted();

            ApplicationDbContext identityContext = (ApplicationDbContext)app
                .ApplicationServices.GetService(typeof(ApplicationDbContext));
            identityContext.Database.Migrate();
            identityContext.Database.EnsureDeleted();
        }

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = (ApplicationDbContext)app
                .ApplicationServices.GetService(typeof(ApplicationDbContext));
            context.Database.Migrate();

            var techniqueRepository = new TechniqueRepository(context);
            if (techniqueRepository.Techniques.Count() == 0)
            {
                techniqueRepository.Save(new TechniqueDict() { Name = "Olej na płótnie", CodeName = "OP" });
                techniqueRepository.Save(new TechniqueDict() { Name = "Akwarele", CodeName = "WP" });
                techniqueRepository.Save(new TechniqueDict() { Name = "Szkic", CodeName = "SK" });
                techniqueRepository.Save(new TechniqueDict() { Name = "Ceramika", CodeName = "PO" });
            }

            context.SaveChanges();

            AppIdentityDbContext identityContext = (AppIdentityDbContext)app
                .ApplicationServices.GetService(typeof(AppIdentityDbContext));
            identityContext.Database.Migrate();

            RoleManager<IdentityRole> roleManager =
                (RoleManager<IdentityRole>)app.ApplicationServices.GetService(typeof(RoleManager<IdentityRole>));
            if (!await roleManager.RoleExistsAsync("admin"))
                await roleManager.CreateAsync(new IdentityRole("admin"));
            if (!await roleManager.RoleExistsAsync("editor"))
                await roleManager.CreateAsync(new IdentityRole("editor"));
            if (!await roleManager.RoleExistsAsync("user"))
                await roleManager.CreateAsync(new IdentityRole("user"));

            UserManager<User> userManager = (UserManager<User>)app.ApplicationServices.GetService(typeof(UserManager<User>));
            bool adminExists = false;
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "admin"))
                {
                    adminExists = true;
                    break;
                }
            }
            if (!adminExists)
            {
                User user = new User { UserName = "Admin", Email = "admin@a.a", EmailConfirmed = true };
                var passwordHasher = (IPasswordHasher<User>)app.ApplicationServices.GetService(typeof(IPasswordHasher<User>));
                user.PasswordHash = passwordHasher.HashPassword(user, "Admin1.");
                IdentityResult creationResult = await userManager.CreateAsync(user);
                if (creationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                    await userManager.AddToRoleAsync(user, "editor");
                    await userManager.AddToRoleAsync(user, "user");
                }
            }

            identityContext.SaveChanges();
        }
    }
}
