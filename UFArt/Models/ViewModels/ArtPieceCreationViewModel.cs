using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models.ViewModels
{
    public class ArtPieceCreationViewModel
    {
        public ITechniqueRepository TechniqueRepository { get; set; }
        public ArtPiece ArtPiece { get; set; }
        public List<string> Techniques()
        {
            List<string> techniques = new List<string>();
            foreach(var technique in TechniqueRepository.Techniques)
            {
                techniques.Add(technique.Name);
            }
            return techniques;
        }

        public ArtPieceCreationViewModel() { }

        public ArtPieceCreationViewModel(ITechniqueRepository repo) => TechniqueRepository = repo;
    }
}
