using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Gallery
{
    public class TechniquesViewModel : ViewModel
    {
        public IQueryable<Technique> Techniques { get; set; }

        public TechniquesViewModel(ITextAssetsRepository textRepo, IQueryable<Technique> techniques)
            : base(textRepo) => Techniques = techniques;
    }
}
