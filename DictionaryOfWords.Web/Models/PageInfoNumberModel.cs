using DictionaryOfWords.Web.Models.Language;
using DictionaryOfWords.Web.Models.Word;
using DictionaryOfWords.Web.Models.WordTranslation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class PageInfoNumberModel : PagedResultList
    {
        [JsonProperty(PropertyName = "wordFilter")]
        public WordFilterModel WordFilter { get; set; }

        [JsonProperty(PropertyName = "languageFilter")]
        public LanguageFilterModel LanguageFilter { get; set; }

        [JsonProperty(PropertyName = "wordTranslationFilter")]
        public WordTranslationFilterModel WordTranslationFilter { get; set; }
    }
}
