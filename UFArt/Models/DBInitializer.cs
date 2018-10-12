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

            var techniques = new List<TechniqueDict>()
            {
                new TechniqueDict { Name = "Olej na płótnie" },
                new TechniqueDict { Name = "Akwarele" }
            };

            if(!context.Techniques.Any())
            {
                context.Techniques.AddRange(techniques);
                context.SaveChanges();
            }

            if(!context.Paintings.Any())
            {
                context.Paintings.AddRange(
                    new Painting { Name = "Bohomaz1", ImageUrl = @"/img/gallery/oil_paintings/painting1.png", ForSale = false, Technique = techniques[0], Dimensions = "100x100cm" },
                    new Painting { Name = "Bohomaz2", ImageUrl = @"/img/gallery/oil_paintings/painting2.png", ForSale = false, Technique = techniques[0], Dimensions = "10x20cm" },
                    new Painting { Name = "Bohomaz3", ImageUrl = @"/img/gallery/oil_paintings/painting3.png", ForSale = true, Technique = techniques[0], Dimensions = "200x300cm" },
                    new Painting { Name = "Bohomaz4", ImageUrl = @"/img/gallery/oil_paintings/painting4.png", ForSale = true, Technique = techniques[0], Dimensions = "15x15cm" },
                    new Painting { Name = "Obraz1", ImageUrl = @"/img/gallery/oil_paintings/painting1.png", ForSale = false, Technique = techniques[1], Dimensions = "A4" },
                    new Painting { Name = "Obraz2", ImageUrl = @"/img/gallery/oil_paintings/painting2.png", ForSale = false, Technique = techniques[1], Dimensions = "A3" },
                    new Painting { Name = "Obraz3", ImageUrl = @"/img/gallery/oil_paintings/painting3.png", ForSale = true, Technique = techniques[1], Dimensions = "A4" },
                    new Painting { Name = "Obraz4", ImageUrl = @"/img/gallery/oil_paintings/painting4.png", ForSale = true, Technique = techniques[1], Dimensions = "A4" }
                );
            }

            if(!context.Potteries.Any())
            {
                context.Potteries.AddRange(
                    new Pottery { Name = "Dzbanek", ForSale = true },
                    new Pottery { Name = "Garnek", ForSale = false }
                );
            }

            if(!context.News.Any())
            {
                context.News.AddRange(
                    new News { Header = "Breaking news!", Text = "These are breaking news",
                        ImageUrl = @"img/gallery/oil_paintings/painting1.png",
                        Timestamp = DateTime.Now },
                    new News
                    { Header = "Even more breaking news!", Text = "Whoa!",
                        ImageUrl = @"img/gallery/oil_paintings/painting2.png",
                        Timestamp = DateTime.Now }
                );
            }

            context.SaveChanges();
        }
    }
}
