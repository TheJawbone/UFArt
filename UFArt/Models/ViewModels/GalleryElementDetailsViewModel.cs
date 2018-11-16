using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.TextAssets;

namespace UFArt.Models.ViewModels
{
    public class GalleryElementDetailsViewModel: ViewModel
    {
        public ArtPiece ArtPiece { get; }
        public Dictionary<string, string> LabelValueDict { get; }
        public string ImageUri { get; }
        public string ReturnUrl { get; }
        public bool ForSale { get; }

        public GalleryElementDetailsViewModel(ITextAssetsRepository textRepository, ArtPiece artPiece, string returnUrl, HttpContext context)
            : base(textRepository)
        {
            LabelValueDict = new Dictionary<string, string>();

            ArtPiece = artPiece;
            ImageUri = artPiece.ImageUri;
            ForSale = artPiece.ForSale;
            ReturnUrl = returnUrl;

            if (artPiece.Name != null) LabelValueDict[textRepository.GetTranslatedValue("name", context)] = artPiece.Name;
            if (artPiece.Description != null) LabelValueDict[textRepository.GetTranslatedValue("description", context)] = artPiece.Description;
            LabelValueDict[textRepository.GetTranslatedValue("technique", context)] = artPiece.Technique;
            if (artPiece.CreationDate != null) LabelValueDict[textRepository.GetTranslatedValue("creation_date", context)] = artPiece.CreationDate;
            if (artPiece.AdditionalInfo != null) LabelValueDict[textRepository.GetTranslatedValue("additional_info", context)] = artPiece.AdditionalInfo;
        }

        public GalleryElementDetailsViewModel(ITextAssetsRepository textRepository) : base(textRepository)
        {
        }
    }
}
