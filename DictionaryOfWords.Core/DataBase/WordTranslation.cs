using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class WordTranslation : IEntity<int>
    {
        public int Id { get; set; }
        public int WordSourceId { get; set; }
        public virtual Word WordSource { get; set; }
        public int WordTranslationId { get; set; }
        public virtual Word WordTranslationValue { get; set; }
        public int LanguageFromId { get; set; }
        public virtual Language LanguageFromWord { get; set; }
        public int LanguageToId { get; set; }
        public virtual Language LanguageToWord { get; set; }
    }
}
