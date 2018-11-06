using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Models.ViewModels
{
    public class GalleryElementDetailsViewModel
    {
        public ArtPiece ArtPiece { get; }
        public Dictionary<string, string> LabelValueDict { get; }
        public string ImageUri { get; }
        public string ReturnUrl { get; }
        public bool ForSale { get; }

        public GalleryElementDetailsViewModel(ArtPiece artPiece, string returnUrl)
        {
            LabelValueDict = new Dictionary<string, string>();

            ArtPiece = artPiece;
            ImageUri = artPiece.ImageUri;
            ForSale = artPiece.ForSale;
            ReturnUrl = returnUrl;

            if (artPiece.Name != null) LabelValueDict["Tytuł"] = artPiece.Name;
            if (artPiece.Description != null) LabelValueDict["Opis"] = artPiece.Description;
            LabelValueDict["Technika"] = artPiece.Technique;
            if (artPiece.CreationDate != null) LabelValueDict["Data powstania"] = artPiece.CreationDate;
            if (artPiece.AdditionalInfo != null) LabelValueDict["Dodatkowe informacje"] = artPiece.AdditionalInfo;
        }
    }
}
