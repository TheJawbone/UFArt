using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Gallery
{
    public class GalleryViewModel : ViewModel
    {
        public IGalleryRepository GalleryRepository { get; set; }

        public GalleryViewModel(ITextAssetsRepository textRepository, IGalleryRepository galleryRepository)
            : base(textRepository)
        {
            GalleryRepository = galleryRepository;
        }
    }
}
