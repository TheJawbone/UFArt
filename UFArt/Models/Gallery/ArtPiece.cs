using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Gallery
{
    public class ArtPiece
    {
        public int ID { get; set; }
        public TextAsset Name { get; set; }
        public TextAsset Description { get; set; }
        public string Dimensions { get; set; }
        public Technique Technique { get; set; }
        public string ImageUri { get; set; }
        public bool ForSale { get; set; }
        public string CreationDate { get; set; }
        public TextAsset AdditionalInfo { get; set; }
    }
}
