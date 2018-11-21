using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class AccountActivationViewModel : ViewModel
    {
        public AccountActivationViewModel() { }

        public AccountActivationViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
