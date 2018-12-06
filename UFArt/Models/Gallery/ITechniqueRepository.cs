using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public interface ITechniqueRepository
    {
        IQueryable<Technique> Techniques { get; }
        bool Save(Technique technique);
        bool Update(Technique technique);
        bool Delete(int id);
    }
}
