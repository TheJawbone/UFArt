using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models
{
    public interface IPotteryRepository
    {
        IQueryable<Pottery> Potteries { get; }
        void Save(Pottery pottery);
        void Delete(int id);
    }
}
