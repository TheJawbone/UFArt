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

        public IQueryable<TechniqueDict> Techniques => _context.Techniques;

        public TechniqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Save(TechniqueDict technique)
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
            ArtPiece artPiece = _context.ArtPieces.Where(p => p.ID == id).FirstOrDefault();
            if (artPiece != null)
            {
                try
                {
                    _context.ArtPieces.Remove(artPiece);
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
