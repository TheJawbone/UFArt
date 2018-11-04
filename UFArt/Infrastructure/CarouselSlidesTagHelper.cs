using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "generate-slides")]
    public class CarouselSlidesTagHelper : TagHelper
    {
        private IGalleryRepository _slidesRepository;

        public CarouselSlidesTagHelper(IGalleryRepository slidesRepository)
        {
            _slidesRepository = slidesRepository;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder result = new TagBuilder("div");
            var divs = new List<TagBuilder>();
            foreach(var artPiece in _slidesRepository.ArtPieces)
            {
                TagBuilder div = new TagBuilder("div");
                string styleStr = string.Format("background-image: url({0}); background-size: cover;", artPiece.ImageUri);
                div.Attributes["style"] = styleStr;
                div.AddCssClass("carousel-item");
                div.AddCssClass("h-100");
                divs.Add(div);
                //< div class="carousel-item h-100 active" style="background-image: url(/img/slide1.png); background-size: cover;">
            }
            if (divs.Count > 0) divs.First().AddCssClass("active");
            foreach(var div in divs)
            {
                result.InnerHtml.AppendHtml(div);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
