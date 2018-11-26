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
    public class TechniqueAddViewModel : ViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Wprowadź polską nazwę techniki")]
        public string NamePl { get; set; }
        [Required(ErrorMessage = "Wprowadź angielską nazwę techniki")]
        public string NameEn { get; set; }

        public TechniqueAddViewModel() { }

        public TechniqueAddViewModel(ITextAssetsRepository textRepo)
            : base(textRepo) { }
    }
}
