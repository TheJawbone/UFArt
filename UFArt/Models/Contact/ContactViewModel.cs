using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Contact
{
    public class ContactViewModel : ViewModel
    {
        [BindNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Wprowadź adres email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Wprowadź numer telefonu")]
        public string Telephone { get; set; }

        public ContactViewModel() { }

        public ContactViewModel(ITextAssetsRepository textRepository)
            : base(textRepository) { }
    }
}
