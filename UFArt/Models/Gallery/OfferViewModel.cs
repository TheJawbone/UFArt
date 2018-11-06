using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public class OfferViewModel
    {
        public int ArtPieceId { get; set; }
        [Required(ErrorMessage = "Wprowadź imię")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Wprowadź adres email")]
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
