using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class WordDeleteModel
    {
        public int Id { get; set; }
        [DisplayName("Наимеенование")]
        public string Name { get; set; }

        [DisplayName("Язык")]
        public int LanguageId { get; set; }

        [DisplayName("Язык")]
        public string LanguageName { get; set; }

        public bool IsDelete { get; set; }
    }
}
