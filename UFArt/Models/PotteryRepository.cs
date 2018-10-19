using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models
{
    public class PotteryRepository : IPotteryRepository
    {
        private ApplicationDbContext _context;

        public PotteryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Pottery> Potteries => _context.Potteries;

        public void Save(Pottery pottery)
        {
            _context.Potteries.AddAsync(pottery);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Pottery pottery = _context.Potteries.Where(p => p.ID == id).FirstOrDefault();
            if(pottery != null)
            {
                _context.Potteries.Remove(pottery);
                _context.SaveChanges();
            }
        }
    }
}
