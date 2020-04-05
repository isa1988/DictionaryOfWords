using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service.Services
{
    public class WordTranslationService : GeneralService<WordTranslation, WordTranslationDto, WordTranslationFilterDto>, IWordTranslationService
    {
        public WordTranslationService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, new WordTranslationDto(), mapper)
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
                List<WordTranslationDto> retList = _mapper.Map<List<WordTranslationDto>>(wordTranslationList);
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
                List<WordTranslationDto> retList = _mapper.Map<List<WordTranslationDto>>(wordTranslationList);
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
                    errors += "Текущий перевод уже есть в базе";
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
                WordTranslationDto dto = _mapper.Map<WordTranslationDto>(value);
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
        
        public override List<WordTranslationDto> GetAllFilter(WordTranslationFilterDto filter)
        {
            if (filter == null)
                return new List<WordTranslationDto>();
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordTranslationList = unitOfWork.WordTranslation.GetAllFilter(filter.WordFrom, filter.LanguageFrom, filter.WordTo, filter.LanguageTo);
                if (wordTranslationList.Count == 0) return new List<WordTranslationDto>();
                return _mapper.Map<List<WordTranslationDto>>(wordTranslationList);
            }
        }

        public override List<WordTranslationDto> GetAllOfPageFilter(WordTranslationFilterDto filter, int pageNumber, int rowCount)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordTranslationList = unitOfWork.WordTranslation.GetAllOfPageFilter(pageNumber, rowCount, filter.WordFrom, filter.LanguageFrom, filter.WordTo, filter.LanguageTo);
                if (wordTranslationList.Count == 0) return new List<WordTranslationDto>();
                return _mapper.Map<List<WordTranslationDto>>(wordTranslationList);
            }
        }
    }
}
