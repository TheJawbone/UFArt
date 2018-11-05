using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public class ArtPiece
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; }
        [Required(ErrorMessage = "Wybierz jedną z dostępnych technik")]
        public string Technique { get; set; }
        public string ImageUri { get; set; }
        [Required]
        public bool ForSale { get; set; }
        [RegularExpression(@"^((((0)[0-9])|((1)[0-2]))(-)\d{4})|(\d{4})$", ErrorMessage = "Wprowadź datę w formacie rrrr lub mm-rrrr")]
        public string CreationDate { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
