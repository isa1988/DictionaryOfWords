using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Services
{
    public class WordTranslationService : GeneralServiceDto<WordTranslation, WordTranslationDto>, IWordTranslationService
    {
        public WordTranslationService(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory, new WordTranslationDto())
        {

        }

        public override List<WordTranslationDto> GetAllOfPage(int pageNumber, int rowCount)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<WordTranslation> wordTranslationList = unitOfWork.WordTranslation.GetAllOfPage(pageNumber, rowCount);
                if (wordTranslationList == null || wordTranslationList.Count == 0)
                {
                    return new List<WordTranslationDto>();
                }
                List<WordTranslationDto> retList = Mapper.Map<List<WordTranslationDto>>(wordTranslationList);
                return retList;
            }
        }
        public override List<WordTranslationDto> GetAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<WordTranslation> wordTranslationList = unitOfWork.WordTranslation.GetAll();
                if (wordTranslationList == null || wordTranslationList.Count == 0)
                {
                    return new List<WordTranslationDto>();
                }
                List<WordTranslationDto> retList = Mapper.Map<List<WordTranslationDto>>(wordTranslationList);
                return retList;
            }
        }
        
        protected override string CheckAndGetErrors(WordTranslationDto value, bool isNew = true)
        {
            string errors = string.Empty;
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                Word word = unitOfWork.Word.GetById(value.WordSourceId);
                if (word == null)
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Слово с какого идет первод не найден";
                }
                word = unitOfWork.Word.GetById(value.WordTranslationId);
                if (word == null)
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Слово на который идет первод не найден";
                }

                Language language = unitOfWork.Language.GetById(value.LanguageFromId);
                if (language == null)
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Язык с которого идет первод не найден";
                }
                language = unitOfWork.Language.GetById(value.LanguageToId);
                if (language == null)
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Язык на который идет первод не найден";
                }
                if (string.IsNullOrEmpty(errors) && unitOfWork.WordTranslation.IsNameReplay(value.Id, value.WordSourceId, value.WordTranslationId, value.LanguageFromId, value.LanguageToId, isNew))
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Текущий язык уже есть в базе";
                }
            }
            return errors;
        }
        public override WordTranslationDto GetByID(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                Word value = unitOfWork.GetRepository<Word>().GetById(id);
                if (value == null) return new WordTranslationDto();
                WordTranslationDto dto = Mapper.Map<WordTranslationDto>(value);
                return dto;

            }
        }

        protected override string CkeckBefforDelet(WordTranslation value)
        {
            return string.Empty;
        }

        protected override string CkeckBefforDeleteList(List<WordTranslation> listVal)
        {
            return string.Empty;
        }

        public List<WordTranslationDto> GetAllFilter(string wordFrom, string languageFrom, string wordTo, string languageTo)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordTranslationList = unitOfWork.WordTranslation.GetAllFilter(wordFrom, languageFrom, wordTo, languageTo);
                if (wordTranslationList.Count == 0) return new List<WordTranslationDto>();
                return AutoMapper.Mapper.Map<List<WordTranslationDto>>(wordTranslationList);
            }
        }

        public List<WordTranslationDto> GetAllOfPageFilter(int pageNumber, int rowCount, string wordFrom, string languageFrom, string wordTo, string languageTo)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordTranslationList = unitOfWork.WordTranslation.GetAllOfPageFilter(pageNumber, rowCount, wordFrom, languageFrom, wordTo, languageTo);
                if (wordTranslationList.Count == 0) return new List<WordTranslationDto>();
                return AutoMapper.Mapper.Map<List<WordTranslationDto>>(wordTranslationList);
            }
        }
    }
}
