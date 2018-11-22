using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Newsfeed
{
    public class GallerySuccessViewModel : ViewModel
    {
        public int Id { get; set; }

        public GallerySuccessViewModel() { }

        public GallerySuccessViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
