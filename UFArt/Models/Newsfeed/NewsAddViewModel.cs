using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Newsfeed
{
    public class NewsAddViewModel : ViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Wprowadź tekst wpisu aktualności")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Wprowadź tytuł wpisu aktualności")]
        public string Header { get; set; }
        public string ImageUri { get; set; }
        public DateTime Timestamp { get; set; }

        public NewsAddViewModel() { }

        public NewsAddViewModel(ITextAssetsRepository textRepository)
            : base(textRepository) { }

        public NewsAddViewModel(News news, HttpContext context, ITextAssetsRepository textRepository)
            : base(textRepository)
        {
            ID = news.ID;
            Text = TextRepository.GetTranslatedValue(news.Text, context);
            Header = TextRepository.GetTranslatedValue(news.Header, context);
            ImageUri = news.ImageUrl;
            Timestamp = news.Timestamp;
        }
    }
}
