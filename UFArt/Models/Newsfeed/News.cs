using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;

namespace UFArt.Models.Newsfeed
{
    public class News
    {
        public int ID { get; set; }
        public TextAsset Header { get; set; }
        public TextAsset Text { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
