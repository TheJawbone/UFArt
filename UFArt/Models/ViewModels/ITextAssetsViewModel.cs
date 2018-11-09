using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.ViewModels
{
    public interface ITextAssetsViewModel
    {
        string GetText(string key);
    }
}
