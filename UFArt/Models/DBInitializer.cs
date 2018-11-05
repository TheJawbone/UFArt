using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
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
        }

        public static void MigrateDatabase(IApplicationBuilder app)
        {
            ApplicationDbContext context = (ApplicationDbContext)app
                .ApplicationServices.GetService(typeof(ApplicationDbContext));
            context.Database.Migrate();
            context.SaveChanges();

            AppIdentityDbContext identityContext = (AppIdentityDbContext)app
                .ApplicationServices.GetService(typeof(AppIdentityDbContext));
            identityContext.Database.Migrate();
            identityContext.SaveChanges();
        }
    }
}
