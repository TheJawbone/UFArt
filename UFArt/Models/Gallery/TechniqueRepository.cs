using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public class TechniqueRepository : ITechniqueRepository
    {
        private ApplicationDbContext _context;

        public IQueryable<Technique> Techniques => _context.Techniques.Include(t => t.Name);

        public TechniqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Save(Technique technique)
        {
            try
            {
                _context.Techniques.Add(technique);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return false;
                }
                throw;
            }
        }

        public bool Delete(int id)
        {
            Technique technique = _context.Techniques.Where(t => t.ID == id).Include(t => t.Name).FirstOrDefault();
            if (technique != null)
            {
                try
                {
                    var asset = _context.TextAssets.Where(ta => ta.Id == technique.Name.Id).FirstOrDefault();
                    _context.Techniques.Remove(technique);
                    if (asset != null)
                        _context.TextAssets.Remove(asset);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex is DbUpdateException || ex is DbUpdateConcurrencyException) return false;
                    throw;
                }
            }
            return false;
        }
    }
}
