using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Newsfeed
{
    public class NewsOld
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Wprowadź tytuł")]
        public string Header { get; set; }
        [Required(ErrorMessage = "Wprowadź treść aktualności")]
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
