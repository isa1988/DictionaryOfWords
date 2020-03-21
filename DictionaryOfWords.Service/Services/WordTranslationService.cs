using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Text;

namespace DictionaryOfWords.Service.Services
{
    public class WordTranslationService : GeneralServiceDto<WordTranslation, WordTranslationDto>, IWordTranslationService
    {
        public WordTranslationService(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory, new WordTranslationDto())
        {

        }
        public override string CheckAndGetErrors(WordTranslationDto value, bool isNew = true)
        {
            string errors = string.Empty;
            //value.WordSourceId
            return errors;
        }
    }
}
