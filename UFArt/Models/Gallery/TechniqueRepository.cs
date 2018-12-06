using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Configuration;

namespace UFArt.Models.Gallery
{
    public class TechniqueRepository : ITechniqueRepository
    {
        private ApplicationDbContext _context;
        private readonly IOptions<StorageSettings> _storageSettings;

        public IQueryable<Technique> Techniques => _context.Techniques.Include(t => t.Name);

        public TechniqueRepository(ApplicationDbContext context, IOptions<StorageSettings> storageSettings)
        {
            _context = context;
            _storageSettings = storageSettings;
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

        public bool Update(Technique technique)
        {
            try
            {
                _context.Techniques.Update(technique);
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
            var technique = _context.Techniques.Where(t => t.ID == id).Include(t => t.Name).FirstOrDefault();
            if (technique != null)
            {
                try
                {
                    var storageFacade = new StorageFacade(_storageSettings);
                    var imageUris = _context.ArtPieces.Where(ap => ap.Technique.ID == technique.ID).Select(ap => ap.ImageUri).ToList();
                    var asset = _context.TextAssets.Where(ta => ta.Id == technique.Name.Id).FirstOrDefault();
                    _context.Techniques.Remove(technique);
                    if (asset != null)
                        _context.TextAssets.Remove(asset);
                    foreach (var imageUri in imageUris)
                    {
                        storageFacade.DeleteImageBlob(imageUri);
                    }
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
