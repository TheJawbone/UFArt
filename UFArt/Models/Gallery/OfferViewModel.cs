using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Identity;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Gallery
{
    public class OfferViewModel : ViewModel
    {
        public int ArtPieceId { get; set; }
        [Required(ErrorMessage = "Wprowadź imię")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Wprowadź adres email")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public User User { get; set; }

        public OfferViewModel() { }

        public OfferViewModel(ITextAssetsRepository textRepository)
            : base(textRepository) { }
    }
}
