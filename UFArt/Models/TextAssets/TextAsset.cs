using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.TextAssets
{
    public class TextAsset
    {
        [BindNever]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value_pl {get; set;}
        public string Value_en { get; set; }
    }
}
