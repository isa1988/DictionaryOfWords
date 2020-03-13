using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class WordModel : PageInfoModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Наимеенование")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Язык")]
        public int LanguageId { get; set; }

        [DisplayName("Язык")]
        public SelectList LanguageList { get; set; }
        public string LanguageName { get; set; }
    }
}
