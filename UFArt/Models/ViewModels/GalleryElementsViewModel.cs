using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.TextAssets;

namespace UFArt.Models.ViewModels
{
    public class GalleryElementsViewModel : ViewModel
    {
        public IEnumerable<ArtPiece> Elements { get; set; }
        public PagingInfo PagingInfo { get; set; }

        GalleryElementsViewModel() { }

        public GalleryElementsViewModel(ITextAssetsRepository textRepository)
            : base(textRepository) { }
    }
}
