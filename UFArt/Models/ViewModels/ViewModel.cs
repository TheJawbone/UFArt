using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;

namespace UFArt.Models.ViewModels
{
    public class ViewModel
    {
        public ITextAssetsRepository TextRepository { get; set; }

        public ViewModel(ITextAssetsRepository textRepository)
        {
            TextRepository = textRepository;
        }

        public ViewModel() { }
    }
}
