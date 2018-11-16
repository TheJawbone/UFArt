using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;

namespace UFArt.Models.ViewModels
{
    public class ActionSuccessViewModel : ViewModel
    {
        public TextAsset Message { get; set; }
        public string ReturnUri { get; set; }

        public ActionSuccessViewModel() { }

        public ActionSuccessViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
