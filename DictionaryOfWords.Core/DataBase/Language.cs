using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class Language : Entity
    {
        public string Name { get; set; }
        public virtual List<Word> Words {get; set; }

        public Language()
        {
            Words = new List<Word>();
        }
    }
}
