using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.TextAssets;

namespace UFArt.Models.ViewModels
{
    public class ArtPieceCreationViewModel : ViewModel
    {
        public int Id { get; set; }
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
        public string Language { get; set; }
        public bool SuccessFlag { get; set; }

        public ITechniqueRepository TechniqueRepository { get; set; }
        
        public List<Technique> Techniques()
        {
            List<Technique> techniques = new List<Technique>();
            foreach(var technique in TechniqueRepository.Techniques)
            {
                techniques.Add(technique);
            }
            return techniques;
        }

        public ArtPieceCreationViewModel() { }

        public ArtPieceCreationViewModel(ITechniqueRepository repo, ITextAssetsRepository textRepository)
            : base(textRepository)
        {
            TechniqueRepository = repo;
            Language = "pl";
        }
    }
}
