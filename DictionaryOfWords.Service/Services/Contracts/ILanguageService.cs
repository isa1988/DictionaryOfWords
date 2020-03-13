using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface ILanguageService : IGeneralServiceDto<Language, LanguageDto>
    {
        Task<EntityOperationResult<Language>> EditItemAsync(LanguageDto basketEditDto);
    }
}
