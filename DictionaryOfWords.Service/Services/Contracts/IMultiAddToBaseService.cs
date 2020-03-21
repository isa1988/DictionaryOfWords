using DictionaryOfWords.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IMultiAddToBaseService
    {
        List<WordTranslationDto> WordTranslations { get; }
        List<WordDto> Words { get; }
        int CountMultiAddToDateBase { get; }

        IEnumerable<int> PreSetMultiAddToDateBase();

        IEnumerable<int> AnalizeDate();

        void MultiAddToDateBase();

        void SetText(string text);
    }
}
