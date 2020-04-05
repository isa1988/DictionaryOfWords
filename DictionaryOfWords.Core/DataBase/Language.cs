using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class Language : EntityBase
    {
        public Language()
        {
            Name = string.Empty;
            Words = new List<Word>();
            WordFromTranslations = new List<WordTranslation>();
            WordToTranslations = new List<WordTranslation>();
        }
        public string Name { get; set; }
        public virtual List<Word> Words {get; set; }
        public virtual List<WordTranslation> WordFromTranslations { get; set; }
        public virtual List<WordTranslation> WordToTranslations { get; set; }

    }
}
