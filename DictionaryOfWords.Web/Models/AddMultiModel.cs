using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public class AddMultiModel
    {
        public AddMultiModel()
        {
            WordMultiModelList = new List<WordMultiModel>();
            WordModelList = new List<WordModel>();
        }

        public List<WordMultiModel> WordMultiModelList { get; set; }
        public List<WordModel> WordModelList { get; set; }

        public bool IsEnd { get; set; }

        [Required]
        [DisplayName("Текст который надо анализировать")]
        public string Text { get; set; }
    }
}
