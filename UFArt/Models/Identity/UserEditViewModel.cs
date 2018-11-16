using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class UserEditViewModel : ViewModel
    {
        public User User { get; set; }

        public UserEditViewModel(User user, ITextAssetsRepository textRepo)
            : base(textRepo) => User = user;
    }
}
