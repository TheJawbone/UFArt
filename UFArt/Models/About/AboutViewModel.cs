using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.About
{
    public class AboutViewModel : ViewModel
    {
        public AboutViewModel(ITextAssetsRepository repo) : base(repo) { }
        public AboutViewModel() { }
        [BindNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Wprowadź treść")]
        public string Text { get; set; }
        public string ImageUri { get; set; }
        public bool SuccessFlag { get; set; }
        public string Language { get; set; }
    }
}
