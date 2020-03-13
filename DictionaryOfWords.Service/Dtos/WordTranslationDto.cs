using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Dtos
{
    public class WordTranslationDto
    {
        public int WordSourceId { get; set; }
        public virtual WordDto WordSource { get; set; }
        public int WordTranslationId { get; set; }
        public virtual WordDto WordTranslationValue { get; set; }
        public int LanguageFromId { get; set; }
        public virtual LanguageDto LanguageFromWord { get; set; }
        public int LanguageToId { get; set; }
        public virtual LanguageDto LanguageToWord { get; set; }
    }
}
