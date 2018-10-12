using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;

namespace UFArt.Models
{
    public interface IDataRepository
    {
        IQueryable<TechniqueDict> Techniques { get; }
        IQueryable<Painting> OilPaintings { get; }
        IQueryable<Painting> WatercolorPaintings { get; }
        IQueryable<Pottery> Potteries { get; }
        IQueryable<News> News { get; }
    }
}
