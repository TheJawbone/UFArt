using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class ChangeAccountDataViewModel : ViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public ChangeAccountDataViewModel() { }

        public ChangeAccountDataViewModel(ITextAssetsRepository textRepo, User user)
            : base(textRepo)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.UserName;
            PhoneNumber = user.PhoneNumber;
        }
    }
}
