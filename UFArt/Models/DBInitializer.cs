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

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = (ApplicationDbContext)app
                .ApplicationServices.GetService(typeof(ApplicationDbContext));
            context.Database.Migrate();

            if (context.ArtPieces.Count() == 0)
            {
                context.ArtPieces.Add(new ArtPiece
                {
                    Name = "Test",
                    Description = "Test",
                    Technique = "Test",
                    CreationDate = "01-1990",
                    ForSale = true,
                    ImageUri = @"img/gallery/oil_paintings/painting1.png"
                });
            }

            context.SaveChanges();
        }
    }
}
