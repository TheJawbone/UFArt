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
        public List<string> Lables { get; }
        public List<string> Values { get; }
        public string ImageUri { get; }

        public GalleryElementDetailsViewModel(ArtPiece artPiece)
        {
            ArtPiece = artPiece;
            ImageUri = artPiece.ImageUri;

            if (artPiece.Name != null)
            {
                Lables.Add("Tytuł");
                Values.Add(artPiece.Name);
            }

            if (artPiece.Description != null)
            {
                Lables.Add("Opis");
                Values.Add(artPiece.Description);
            }

            Lables.Add("Technika");
            Values.Add(artPiece.Technique);

            if (artPiece.CreationDate != null)
            {
                Lables.Add("Data powstania");
                Values.Add(string.Format("{0}.{1}", artPiece.CreationDate.Month, artPiece.CreationDate.Year));
            }

            if(artPiece.ForSale) Lables.Add("Dostępny sprzedaż");
        }
    }
}
