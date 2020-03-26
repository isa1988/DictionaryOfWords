using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class WordFilterModel
    {
        [DisplayName("Наименование")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DisplayName("Язык")]
        [JsonProperty(PropertyName = "language")]
        public string LanguageName { get; set; }
    }
}
