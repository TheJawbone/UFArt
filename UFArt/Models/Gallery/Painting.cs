using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Gallery
{
    public class Painting : ArtPiece
    {
        public string Dimensions { get; set; }
        public TechniqueDict Technique { get; set; }
    }
}
