using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class RolesManageViewModel : ViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; private set; }

        public RolesManageViewModel(IEnumerable<IdentityRole> roles, ITextAssetsRepository textRepo)
            : base(textRepo) => Roles = roles;
    }
}
