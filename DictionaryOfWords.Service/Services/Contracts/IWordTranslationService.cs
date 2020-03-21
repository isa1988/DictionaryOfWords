using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IWordTranslationService : IGeneralServiceDto<WordTranslation, WordTranslationDto>
    {
    }
}
