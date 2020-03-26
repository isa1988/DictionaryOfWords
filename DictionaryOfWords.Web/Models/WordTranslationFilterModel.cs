using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class WordTranslationFilterModel
    {
        [DisplayName("Слово")]
        [JsonProperty(PropertyName = "wordFrom")]
        public string WordFrom { get; set; }

        [DisplayName("Язык")]
        [JsonProperty(PropertyName = "languageFrom")]
        public string LanguageFrom { get; set; }

        [DisplayName("Слово перевод")]
        [JsonProperty(PropertyName = "wordTo")]
        public string WordTo { get; set; }

        [DisplayName("Язык перевод")]
        [JsonProperty(PropertyName = "languageTo")]
        public string LanguageTo { get; set; }
    }
}
