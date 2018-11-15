using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Newsfeed
{
    public class NewsfeedViewModel : ViewModel
    {
        public INewsfeedRepository Repo { get; set; }
        public IEnumerable<News> News => Repo?.News.Take(NewsDisplayed);
        public int NewsDisplayed { get; set; }
        public int NewsIncrement { get; set; }

        public NewsfeedViewModel() { }

        public NewsfeedViewModel(ITextAssetsRepository textRepository, INewsfeedRepository repo, int newsDisplayed = 2, int newsIncrement = 2)
            : base(textRepository)
        {
            Repo = repo;
            NewsDisplayed = newsDisplayed;
            NewsIncrement = newsIncrement;
        }
    }
}
