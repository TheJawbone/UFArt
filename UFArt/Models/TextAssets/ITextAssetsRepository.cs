﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.TextAssets
{
    public interface ITextAssetsRepository
    {
        string GetTranslatedValue(string key, HttpContext context);
        string GetTranslatedValue(TextAsset asset, HttpContext context);
        string GetTranslatedValue(string key, string languageCode);
        string GetTranslatedValue(TextAsset asset, string languageCode);
        TextAsset GetAsset(string key);
        IQueryable<TextAsset> GetAssets(string key);
        void SaveAsset(TextAsset asset);
        bool UpdateAsset(TextAsset asset);
    }
}
