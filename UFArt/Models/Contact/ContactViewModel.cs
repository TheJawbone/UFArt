using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Models.Contact
{
    public class ContactViewModel : ITextAssetsViewModel
    {
        private readonly ITextAssetsRepository _repo;
        private readonly HttpContext _context;

        public ContactViewModel(ITextAssetsRepository repo, HttpContext context)
        {
            _repo = repo;
            _context = context;
        }

        public string GetText(string key)
        {
            return _repo.GetTranslatedValue(key, _context);
        }
    }
}
