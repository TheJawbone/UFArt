using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class UserViewModel : ViewModel
    {
        public User User { get; set; }

        public UserViewModel() { }

        public UserViewModel(ITextAssetsRepository textRepo, User user)
            : base(textRepo) => User = user;
    }
}
