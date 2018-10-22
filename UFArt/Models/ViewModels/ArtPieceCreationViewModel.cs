using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models.ViewModels
{
    public class ArtPieceCreationViewModel
    {
        private ITechniqueRepository _repo;
        public ArtPiece ArtPiece { get; set; }
        public List<string> Techniques()
        {
            List<string> techniques = new List<string>();
            foreach(var technique in _repo.Techniques)
            {
                techniques.Add(technique.Name);
            }
            return techniques;
        }

        public ArtPieceCreationViewModel() { }

        public ArtPieceCreationViewModel(ITechniqueRepository repo) => _repo = repo;
    }
}
