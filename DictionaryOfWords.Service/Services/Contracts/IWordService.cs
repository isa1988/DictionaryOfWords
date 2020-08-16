using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IWordService : IGeneralServiceWithId<Word, WordDto, WordFilterDto>
    {
        Task<EntityOperationResult<Word>> EditItemAsync(WordDto basketEditDto);

        List<WordDto> GetAllWordsForLanguage(int languageId);
    }
}
