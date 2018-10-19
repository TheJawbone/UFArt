using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models
{
    public class WatercolorPaintingsRepository : IWatercolorPaintingsRepository
    {
        private ApplicationDbContext _context;

        public WatercolorPaintingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Painting> WatercolorPaintings =>
            _context.Paintings
            .Where(p => p.Technique.Name == "Akwarele")
            .Include(p => p.Technique);

        public void Save(Painting painting)
        {
            _context.Paintings.AddAsync(painting);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Painting painting = _context.Paintings.Where(p => p.ID == id).FirstOrDefault();
            if(painting != null)
            {
                _context.Paintings.Remove(painting);
                _context.SaveChanges();
            }
        }
    }
}
