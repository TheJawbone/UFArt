using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public interface IGalleryRepository
    {
        IQueryable<ArtPiece> ArtPieces { get; }
        bool Save(ArtPiece artPiece);
        bool Update(ArtPiece artPiece);
        Task<bool> Delete(int id);
    }
}
