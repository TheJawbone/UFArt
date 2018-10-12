using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Newsfeed
{
    public class News
    {
        public int ID { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
