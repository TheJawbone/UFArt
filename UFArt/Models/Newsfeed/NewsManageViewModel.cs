using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Newsfeed
{
    public class NewsManageViewModel : ViewModel
    {
        public INewsfeedRepository NewsRepo { get; set; }

        public NewsManageViewModel(INewsfeedRepository newsRepo, ITextAssetsRepository textRepo)
            : base(textRepo) => NewsRepo = newsRepo;
    }
}
