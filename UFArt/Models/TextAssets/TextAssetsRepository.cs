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

        public string GetTranslatedValue(string key, HttpContext context)
        {
            string language = context.Session.GetString("language");
            switch (language)
            {
                case "pl":
                    return _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_pl).FirstOrDefault();
                case "en":
                    return _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_en).FirstOrDefault();
                default:
                    return _context.TextAssets.Where(ta => ta.Key == key).Select(ta => ta.Value_pl).FirstOrDefault();
            }
        }

        public async void SaveAsset(TextAsset assetToAdd)
        {
            TextAsset asset = _context.TextAssets.Where(a => a.Key == assetToAdd.Key).FirstOrDefault();
            if(asset == null)  await _context.TextAssets.AddAsync(assetToAdd);
            else
            {
                asset.Value_pl = assetToAdd.Value_pl;
                asset.Value_en = assetToAdd.Value_en;
                _context.TextAssets.Update(asset);
            }
            _context.SaveChanges();
        }
    }
}
