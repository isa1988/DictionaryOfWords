using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class DeleteMultiModel : PageInfoModel
    {
        public List<LanguageModel> LanguageModels { get; set; }

        public List<WordTranslationModel> WordTranslationModels { get; set; }

        public List<WordDeleteModel> WordModels { get; set; }


        public DeleteMultiModel()
        {
            WordTranslationModels = new List<WordTranslationModel>();
            LanguageModels = new List<LanguageModel>();
            WordModels = new List<WordDeleteModel>();
        }
    }
}
