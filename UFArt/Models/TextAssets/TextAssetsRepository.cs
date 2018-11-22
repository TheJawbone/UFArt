using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.TextAssets
{
    public class TextAssetsRepository : ITextAssetsRepository
    {
        private readonly ApplicationDbContext _context;

        public TextAssetsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TextAsset GetAsset(string key) =>
            _context.TextAssets.Where(ta => ta.Key == key).FirstOrDefault();

        public IQueryable<TextAsset> GetAssets(string key) =>
            _context.TextAssets.Where(ta => ta.Key == key);

        public string GetTranslatedValue(string key, HttpContext context)
        {
            string language = context.Session.GetString("language");
            if (language == null) context.Session.SetString("language", "pl");
            string asset = null;
            switch (language)
            {
                case "pl":
                    asset = _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_pl).FirstOrDefault();
                    break;
                case "en":
                    asset = _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_en).FirstOrDefault();
                    break;
            }
            if (asset == null) return _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_pl).FirstOrDefault();
            else return asset;
        }

        public string GetTranslatedValue(string key, string languageCode)
        {
            string asset = null;
            switch (languageCode)
            {
                case "pl":
                    asset = _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_pl).FirstOrDefault();
                    break;
                case "en":
                    asset = _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_en).FirstOrDefault();
                    break;
            }
            if (asset == null) return _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_pl).FirstOrDefault();
            else return asset;
        }

        public string GetTranslatedValue(TextAsset asset, HttpContext context)
        {
            string language = context.Session.GetString("language");
            switch (language)
            {
                case "pl":
                    return asset.Value_pl;
                case "en":
                    return asset.Value_en;
                default:
                    return asset.Value_pl;
            }
        }

        public string GetTranslatedValue(TextAsset asset, string languageCode)
        {
            switch (languageCode)
            {
                case "pl":
                    return asset.Value_pl;
                case "en":
                    return asset.Value_en;
                default:
                    return asset.Value_pl;
            }
        }

        public async void SaveAsset(TextAsset assetToAdd)
        {
            TextAsset asset = _context.TextAssets.Where(a => a.Id == assetToAdd.Id).FirstOrDefault();
            if(asset == null)  await _context.TextAssets.AddAsync(assetToAdd);
            else
            {
                asset.Value_pl = assetToAdd.Value_pl;
                asset.Value_en = assetToAdd.Value_en;
                _context.TextAssets.Update(asset);
            }
            _context.SaveChanges();
        }

        public async void DeleteAsset(TextAsset assetToDelete)
        {
            _context.TextAssets.Remove(assetToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
