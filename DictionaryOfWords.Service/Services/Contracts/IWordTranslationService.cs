using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IWordTranslationService : IGeneralServiceWithId<WordTranslation, WordTranslationDto, WordTranslationFilterDto>
    {

    }
}
