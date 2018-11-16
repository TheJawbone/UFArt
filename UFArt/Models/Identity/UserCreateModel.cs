using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Identity
{
    public class UserCreateModel : ViewModel
    {
        [Required(ErrorMessage = "Wprowadź nazwę użytkownka")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Wprowadź adres e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wprowadź hasło")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Wprowadź potwierdzenie hasła")]
        public string PasswordConfirmation { get; set; }

        public UserCreateModel() { }

        public UserCreateModel(ITextAssetsRepository textRepository)
            : base(textRepository) { }
    }
}
