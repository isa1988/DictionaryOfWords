using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class WordMultiModel
    {
        public int Id { get; set; }
        public WordModel WordFrom { get; set; }
        public int WordFromId { get; set; }

        public WordModel WordTo { get; set; }
        public int WordToId { get; set; }

        public LanguageModel LanguageFrom { get; set; }
        public int LanguageFromId { get; set; }
        public LanguageModel LanguageTo { get; set; }
        public int LanguageToId { get; set; }
    }
}
