using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class RoleCreateViewModel : ViewModel
    {
        public string RoleName { get; set; }

        public RoleCreateViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
