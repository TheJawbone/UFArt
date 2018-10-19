using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Newsfeed
{
    public class NewsfeedRepository : INewsfeedRepository
    {
        private ApplicationDbContext _context;

        public IQueryable<News> News => _context.News;

        public bool Save(News news)
        {
            try
            {
                _context.News.Add(news);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException) return false;
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            News news = await _context.News.Where(n => n.ID == id).FirstOrDefaultAsync();
            if (news != null)
            {
                try
                {
                    _context.Remove(news);
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
