using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class ViewListModel : PagedResultList
    {
        [JsonProperty(PropertyName = "languageModels")]
        public List<LanguageModel> LanguageModels { get; set; }

        [JsonProperty(PropertyName = "wordTranslationModels")]
        public List<WordTranslationModel> WordTranslationModels { get; set; }
        
        [JsonProperty(PropertyName = "wordModels")]
        public List<WordDeleteModel> WordModels { get; set; }
        
        [JsonProperty(PropertyName = "isDelete")]
        public bool IsDelete { get; set; }
        
        public ViewListModel()
        {
            WordTranslationModels = new List<WordTranslationModel>();
            LanguageModels = new List<LanguageModel>();
            WordModels = new List<WordDeleteModel>();
        }
    }
}
