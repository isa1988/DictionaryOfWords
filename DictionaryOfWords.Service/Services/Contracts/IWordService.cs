using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IWordService : IGeneralServiceDto<Word, WordDto>
    {
        Task<EntityOperationResult<Word>> EditItemAsync(WordDto basketEditDto);

        List<WordDto> GetAllWordsForLanguage(int languageId);

        List<LanguageDto> GetLanguageList();
    }
}
