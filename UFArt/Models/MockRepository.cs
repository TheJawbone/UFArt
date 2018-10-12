using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;

namespace UFArt.Models
{
    public class MockRepository : IDataRepository
    {
        public IQueryable<TechniqueDict> Techniques => new List<TechniqueDict>
        {
            new TechniqueDict { Name = "Olej na płótnie" },
            new TechniqueDict { Name = "Akwarele" }
        }.AsQueryable();

        public IQueryable<Painting> OilPaintings => new List<Painting>
        {
            new Painting { Name = "Obraz1", ImageUrl = @"/img/gallery/oil_paintings/painting1.png",
                ForSale = false, Technique = new TechniqueDict { Name = "Olej na płótnie" },
                Dimensions = "100x100cm" },
            new Painting { Name = "Obraz2", ImageUrl = @"/img/gallery/oil_paintings/painting2.png",
                ForSale = false, Technique = new TechniqueDict { Name = "Olej na płótnie" },
                Dimensions = "10x20cm" },
            new Painting { Name = "Obraz3", ImageUrl = @"/img/gallery/oil_paintings/painting3.png",
                ForSale = true, Technique = new TechniqueDict { Name = "Olej na płótnie" },
                Dimensions = "200x300cm" },
            new Painting { Name = "Obraz4", ImageUrl = @"/img/gallery/oil_paintings/painting4.png",
                ForSale = true, Technique = new TechniqueDict { Name = "Olej na płótnie" },
                Dimensions = "15x15cm" }
        }.AsQueryable();

        public IQueryable<Painting> WatercolorPaintings => new List<Painting>
        {
            new Painting { Name = "Obraz1", ImageUrl = @"/img/gallery/oil_paintings/painting1.png",
                ForSale = false, Technique = new TechniqueDict { Name = "Akwarela" },
                Dimensions = "A4" },
            new Painting { Name = "Obraz2", ImageUrl = @"/img/gallery/oil_paintings/painting2.png",
                ForSale = false, Technique = new TechniqueDict { Name = "Akwarela" },
                Dimensions = "A3" },
            new Painting { Name = "Obraz3", ImageUrl = @"/img/gallery/oil_paintings/painting3.png",
                ForSale = true, Technique = new TechniqueDict { Name = "Akwarela" },
                Dimensions = "A4" },
            new Painting { Name = "Obraz4", ImageUrl = @"/img/gallery/oil_paintings/painting4.png",
                ForSale = true, Technique = new TechniqueDict { Name = "Akwarela" },
                Dimensions = "A4" }
        }.AsQueryable();

        public IQueryable<Pottery> Potteries => new List<Pottery>
        {
            new Pottery { Name = "Dzbanek", ForSale = true },
            new Pottery { Name = "Garnek", ForSale = false}
        }.AsQueryable();

        public IQueryable<News> News => new List<News>
        {
            new News { Header = "Breaking news!", Text = "These are breaking news",
                ImageUrl = @"img/gallery/oil_paintings/painting1.png",
                Timestamp = DateTime.Now },
            new News { Header = "Even more breaking news!", Text = "Whoa!",
                ImageUrl = @"img/gallery/oil_paintings/painting2.png",
                Timestamp = DateTime.Now }
        }.AsQueryable();
    }
}
