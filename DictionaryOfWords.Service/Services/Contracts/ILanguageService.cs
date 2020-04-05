using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface ILanguageService : IGeneralService<Language, LanguageDto, LanguageFilterDto>
    {
        Task<EntityOperationResult<Language>> EditItemAsync(LanguageDto basketEditDto);
    }
}
