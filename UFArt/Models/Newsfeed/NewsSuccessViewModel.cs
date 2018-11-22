using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Newsfeed
{
    public class NewsSuccessViewModel : ViewModel
    {
        public int Id { get; set; }

        public NewsSuccessViewModel() { }

        public NewsSuccessViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
