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
        public string Language { get; set; }
        public bool SuccessFlag { get; set; }

        public NewsAddViewModel() => Language = "pl";

        public NewsAddViewModel(ITextAssetsRepository textRepository)
            : base(textRepository) => Language = "pl";

        public NewsAddViewModel(News news, string language, ITextAssetsRepository textRepository)
            : base(textRepository)
        {
            ID = news.ID;
            Language = language;
            Text = TextRepository.GetTranslatedValue(news.Text, language);
            Header = TextRepository.GetTranslatedValue(news.Header, language);
            ImageUri = news.ImageUrl;
            Timestamp = news.Timestamp;
        }
    }
}
