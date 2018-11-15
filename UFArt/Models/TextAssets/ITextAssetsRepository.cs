using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.TextAssets
{
    public interface ITextAssetsRepository
    {
        string GetTranslatedValue(string key, HttpContext context);
        TextAsset GetAsset(string key);
        void SaveAsset(TextAsset asset);
    }
}
