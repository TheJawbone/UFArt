using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Identity;
using UFArt.Models.TextAssets;

namespace UFArt.Models.ViewModels
{
    public class GalleryElementDetailsViewModel: ViewModel
    {
        public ArtPiece ArtPiece { get; }
        public User User { get; set; }
        public Dictionary<string, string> LabelValueDict { get; }
        public string ArtPieceId { get; set; }
        public string ImageUri { get; set; }
        public string ReturnUrl { get; set; }
        public bool ForSale { get; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool OfferSuccesfullySent { get; set; }

        public GalleryElementDetailsViewModel() { }

        public GalleryElementDetailsViewModel(ITextAssetsRepository textRepository, ArtPiece artPiece, string returnUrl, HttpContext context)
            : base(textRepository)
        {
            LabelValueDict = new Dictionary<string, string>();

            ArtPiece = artPiece;
            ImageUri = artPiece.ImageUri;
            ForSale = artPiece.ForSale;
            ReturnUrl = returnUrl;

            if (textRepository.GetTranslatedValue(artPiece.Name, context) != null)
                LabelValueDict[textRepository.GetTranslatedValue("name", context)] =
                    TextRepository.GetTranslatedValue(artPiece.Name, context);
            if (textRepository.GetTranslatedValue(artPiece.Description, context) != null)
                LabelValueDict[textRepository.GetTranslatedValue("description", context)] =
                    TextRepository.GetTranslatedValue(artPiece.Description, context);
            LabelValueDict[textRepository.GetTranslatedValue("technique", context)] =
                textRepository.GetTranslatedValue(artPiece.Technique.Name, context);
            if (artPiece.CreationDate != null)
                LabelValueDict[textRepository.GetTranslatedValue("creation_date", context)] =
                    artPiece.CreationDate;
            if (textRepository.GetTranslatedValue(artPiece.AdditionalInfo, context) != null)
                LabelValueDict[textRepository.GetTranslatedValue("additional_info", context)] =
                    TextRepository.GetTranslatedValue(artPiece.AdditionalInfo, context);
        }

        public GalleryElementDetailsViewModel(ITextAssetsRepository textRepository)
            : base(textRepository) { }
    }
}
