using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class Language : Entity
    {
        public string Name { get; set; }
        public virtual List<Word> Words {get; set; }
        public virtual List<WordTranslation> WordTranslations { get; set; }

        public Language()
        {
            Name = string.Empty;
            Words = new List<Word>();
            WordTranslations = new List<WordTranslation>();
        }
    }
}
