using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;

namespace UFArt.Models
{
    public class DataRepository : IDataRepository
    {
        private ApplicationDbContext _context;

        public DataRepository(ApplicationDbContext context)
        {
            _context = context;
            _context.Paintings.Include(p => p.Technique);
        }

        public IQueryable<TechniqueDict> Techniques => _context.Techniques;

        public IQueryable<Pottery> Potteries => _context.Potteries;

        public IQueryable<News> News => _context.News;

        public IQueryable<Painting> OilPaintings =>
            _context.Paintings
            .Where(p => p.Technique.Name == "Olej na płótnie")
            .Include(p => p.Technique);

        public IQueryable<Painting> WatercolorPaintings =>
            _context.Paintings
            .Where(p => p.Technique.Name == "Akwarele")
            .Include(p => p.Technique);
    }
}
