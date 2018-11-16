using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class UsersManageViewModel : ViewModel
    {
        public IEnumerable<User> Users { get; private set; }

        public UsersManageViewModel(IEnumerable<User> users, ITextAssetsRepository textRepo)
            : base(textRepo) => Users = users;
    }
}
