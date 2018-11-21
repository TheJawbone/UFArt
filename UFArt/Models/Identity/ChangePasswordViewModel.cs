using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class ChangePasswordViewModel : ViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Wprowadź stare hasło")]
        [UIHint("password")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Wprowadź nowe hasło")]
        [UIHint("password")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Wprowadź potwierdzenie nowego hasła")]
        [UIHint("password")]
        public string NewPasswordConfirmed { get; set; }

        public ChangePasswordViewModel() { }

        public ChangePasswordViewModel(ITextAssetsRepository textRepo, string id)
            : base(textRepo) => Id = id;
    }
}
