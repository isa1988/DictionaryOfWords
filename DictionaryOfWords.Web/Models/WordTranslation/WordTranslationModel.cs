using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models.WordTranslation
{
    public class WordTranslationModel : PageInfoModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        
        public SelectList WordFromList { get; set; }
        
        [Required]
        [DisplayName("Слово")]
        public int WordFromId { get; set; }
        
        [DisplayName("Слово")]
        [JsonProperty(PropertyName = "wordfromname")]
        public string WordFromName { get; set; }

        public SelectList WordToList { get; set; }
        
        [Required]
        [DisplayName("Слово перевод")]
        public int WordToId { get; set; }
        
        [DisplayName("Слово перевод")]
        [JsonProperty(PropertyName = "wordtoname")]
        public string WordToName { get; set; }

        public SelectList LanguageFromList { get; set; }
        
        [Required]
        [DisplayName("Язык")]
        public int LanguageFromId { get; set; }
        
        [DisplayName("Язык")]
        [JsonProperty(PropertyName = "languagefromname")]
        public string LanguageFromName { get; set; }
        
        public SelectList LanguageToList { get; set; }

        [Required]
        [DisplayName("Язык перевод")]
        public int LanguageToId { get; set; }
        
        [DisplayName("Язык перевод")]
        [JsonProperty(PropertyName = "languagetoname")]
        public string LanguageToName { get; set; }

        [JsonProperty(PropertyName = "isDelete")]
        public bool IsDelete { get; set; }
    }
}
