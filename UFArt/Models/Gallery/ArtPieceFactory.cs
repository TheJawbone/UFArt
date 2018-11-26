using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Gallery
{
    public class ArtPieceFactory
    {
        private readonly IGalleryRepository _galleryRepo;
        private readonly ITextAssetsRepository _textRepo;
        private readonly ITechniqueRepository _techniqueRepo;

        public ArtPieceFactory(IGalleryRepository galleryRepo, ITextAssetsRepository textRepo, ITechniqueRepository techniqueRepo)
        {
            _galleryRepo = galleryRepo;
            _textRepo = textRepo;
            _techniqueRepo = techniqueRepo;
        }

        public ArtPiece CreateArtPiece(ArtPieceCreationViewModel viewModel)
        {
            var artPiece = new ArtPiece();
            var technique = new Technique();

            var nameAsset = new TextAsset() { Key = "art_piece_name" };
            var descriptionAsset = new TextAsset() { Key = "art_piece_description" };
            var additionalInfoAsset = new TextAsset() { Key = "art_piece_additional_info" };

            return AssignValues(viewModel.Language, nameAsset, descriptionAsset, additionalInfoAsset, technique, viewModel, artPiece);
        }

        public ArtPiece UpdateArtPiece(ArtPieceCreationViewModel viewModel, string languageCode)
        {
            var artPiece = _galleryRepo.ArtPieces.Where(ap => ap.ID == viewModel.Id).FirstOrDefault();
            if (artPiece == null) return null;

            var technique = artPiece.Technique;
            var nameAsset = artPiece.Name;
            var descriptionAsset = artPiece.Description;
            var additionalInfoAsset = artPiece.AdditionalInfo;

            return AssignValues(languageCode, nameAsset, descriptionAsset, additionalInfoAsset, technique, viewModel, artPiece);
        }

        public ArtPieceCreationViewModel CreateViewModel(ArtPiece artPiece, ITextAssetsRepository textRepo,
            ITechniqueRepository techniqueRepo, string languageCode, bool success)
        {
            var viewModel = new ArtPieceCreationViewModel(techniqueRepo, textRepo)
            {
                Id = artPiece.ID,
                Name = _textRepo.GetTranslatedValue(artPiece.Name, languageCode),
                Dimensions = artPiece.Dimensions,
                Description = _textRepo.GetTranslatedValue(artPiece.Description, languageCode),
                Technique = _textRepo.GetTranslatedValue(artPiece.Technique.Name, languageCode),
                ImageUri = artPiece.ImageUri,
                ForSale = artPiece.ForSale,
                CreationDate = artPiece.CreationDate,
                AdditionalInfo = _textRepo.GetTranslatedValue(artPiece.AdditionalInfo, languageCode),
                Language = languageCode,
                SuccessFlag = success
            };
            return viewModel;
        }

        private ArtPiece AssignValues(string languageCode, TextAsset nameAsset, TextAsset descriptionAsset,
            TextAsset additionalInfoAsset, Technique technique, ArtPieceCreationViewModel viewModel, ArtPiece artPiece)
        {
            switch (languageCode)
            {
                case "pl":
                    nameAsset.Value_pl = viewModel.Name;
                    descriptionAsset.Value_pl = viewModel.Description;
                    additionalInfoAsset.Value_pl = viewModel.AdditionalInfo;
                    technique = _techniqueRepo.Techniques.Where(t => t.Name.Value_pl == viewModel.Technique).FirstOrDefault();
                    break;
                case "en":
                    nameAsset.Value_en = viewModel.Name;
                    descriptionAsset.Value_en = viewModel.Description;
                    additionalInfoAsset.Value_en = viewModel.AdditionalInfo;
                    technique = _techniqueRepo.Techniques.Where(t => t.Name.Value_en == viewModel.Technique).FirstOrDefault();
                    break;
            }

            _textRepo.SaveAsset(nameAsset);
            _textRepo.SaveAsset(descriptionAsset);
            _textRepo.SaveAsset(additionalInfoAsset);

            artPiece.AdditionalInfo = additionalInfoAsset;
            artPiece.CreationDate = viewModel.CreationDate;
            artPiece.Description = descriptionAsset;
            artPiece.Dimensions = viewModel.Dimensions;
            artPiece.Technique = technique;
            artPiece.ImageUri = viewModel.ImageUri;
            artPiece.ForSale = viewModel.ForSale;
            artPiece.Name = nameAsset;

            return artPiece;
        }
    }
}
