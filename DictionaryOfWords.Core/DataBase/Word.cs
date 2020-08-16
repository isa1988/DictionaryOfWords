using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class Word : IEntity<int>
    {
        public Word()
        {
            WordTranslations = new List<WordTranslation>();
            WordSources = new List<WordTranslation>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }
        public string Pronunciation { get; set; }
        public virtual Language Language { get; set; }
        public virtual List<WordTranslation> WordTranslations { get; set; }
        public virtual List<WordTranslation> WordSources { get; set; }

    }
}
