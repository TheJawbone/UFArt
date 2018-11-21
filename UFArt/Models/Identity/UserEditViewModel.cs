using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class UserEditViewModel : ViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Wprowadź nazwę użytkownika")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Wprowadź adres email")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }

        public UserEditViewModel() { }

        public UserEditViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
