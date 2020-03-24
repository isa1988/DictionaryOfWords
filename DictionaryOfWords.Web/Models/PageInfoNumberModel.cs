using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class PageInfoNumberModel
    {
        [JsonProperty(PropertyName = "pagenumber")]
        public int PageNumber { get; set; }
        //[JsonProperty(PropertyName = "row")]
        //public int Row { get; set; }
    }
}
