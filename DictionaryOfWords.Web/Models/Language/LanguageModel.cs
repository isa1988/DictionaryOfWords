using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models.Language
{
    public class LanguageModel : PageInfoModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Наименование")]
        public string Name { get; set; }

        public bool IsDelete { get; set; }
    }
}
