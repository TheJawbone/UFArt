using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models
{
    public class OilPaintingsRepository : IOilPaintingsRepository
    {
        private ApplicationDbContext _context;

        public OilPaintingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Painting> OilPaintings =>
            _context.Paintings
            .Where(p => p.Technique.Name == "Olej na płótnie")
            .Include(p => p.Technique);

        public void Save(Painting painting)
        {
            _context.Paintings.AddAsync(painting);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Painting painting = _context.Paintings.Where(p => p.ID == id).FirstOrDefault();
            if (painting != null)
            {
                _context.Paintings.Remove(painting);
                _context.SaveChanges();
            }
        }
    }
}
