using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Dtos
{
    public class WordDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Pronunciation { get; set; }

        public int LanguageId { get; set; }

        public LanguageDto Language { get; set; }

        public bool IsAdd { get; set; }

        public List<WordTranslationDto> WordTranslations { get; set; }
        public List<WordTranslationDto> WordSources { get; set; }

        public WordDto()
        {
            WordTranslations = new List<WordTranslationDto>();
            WordSources = new List<WordTranslationDto>();
        }
    }
}
