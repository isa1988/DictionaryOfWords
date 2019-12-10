using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class WordTranslation : Entity
    {
        public int WordSourceId { get; set; }
        public virtual Word WordSource { get; set; }
        public int WordTranslationId { get; set; }
        public virtual Word WordTranslationValue { get; set; }
        public int LanguageId { get; set; }
        public virtual Language LanguageWord { get; set; }
    }
}
