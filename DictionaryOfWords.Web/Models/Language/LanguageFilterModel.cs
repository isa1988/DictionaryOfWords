using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models.Language
{
    public class LanguageFilterModel
    {
        [DisplayName("Наименование")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
