using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models.ViewModels
{
    public class GalleryElementsViewModel
    {
        public IEnumerable<ArtPiece> Elements { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
