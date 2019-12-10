using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Dtos
{
    public class LanguageDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LanguageDto()
        {
            Name = string.Empty;
        }
    }
}
