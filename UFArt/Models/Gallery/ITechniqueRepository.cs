using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public interface ITechniqueRepository
    {
        IQueryable<TechniqueDict> Techniques { get; }
        bool Save(TechniqueDict technique);
        bool Delete(int id);
    }
}
