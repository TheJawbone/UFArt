using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Newsfeed
{
    public class NewsfeedViewModel
    {
        public INewsfeedRepository Repo { get; set; }
        public IEnumerable<News> News => Repo?.News.Take(NewsDisplayed);
        public int NewsDisplayed { get; set; }
        public int NewsIncrement { get; set; }

        public NewsfeedViewModel() { }

        public NewsfeedViewModel(INewsfeedRepository repo, int newsDisplayed = 2, int newsIncrement = 2)
        {
            Repo = repo;
            NewsDisplayed = newsDisplayed;
            NewsIncrement = newsIncrement;
        }
    }
}
