using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public class ArtPiece
    {
        [BindNever]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; }
        public string Technique { get; set; }
        [BindNever]
        public string ImageUri { get; set; }
        public bool ForSale { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
