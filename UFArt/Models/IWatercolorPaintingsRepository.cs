using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models
{
    public interface IWatercolorPaintingsRepository
    {
        IQueryable<Painting> WatercolorPaintings { get; }
        void Save(Painting painting);
        void Delete(int id);
    }
}
