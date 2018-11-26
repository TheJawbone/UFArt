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
        public bool RoleAdded { get; set; }
        public bool RoleUpdated { get; set; }

        public RolesManageViewModel(IEnumerable<IdentityRole> roles, ITextAssetsRepository textRepo)
            : base(textRepo) => Roles = roles;
    }
}
