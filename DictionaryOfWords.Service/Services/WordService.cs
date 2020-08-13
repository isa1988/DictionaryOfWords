﻿using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services
{
    public class WordService : GeneralService<Word, WordDto, WordFilterDto>, IWordService
    {
        public WordService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, new WordDto(), mapper)
        {
        }

        public async Task<EntityOperationResult<Word>> EditItemAsync(WordDto basketEditDto)
        {
            string errors = CheckAndGetErrors(basketEditDto, false);
            if (!string.IsNullOrEmpty(errors))
            {
                return EntityOperationResult<Word>.Failure().AddError(errors);
            }
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    var word = unitOfWork.Word.GetById(basketEditDto.Id);
                    if (word == null)
                    {
                        return EntityOperationResult<Word>.Failure().AddError("Не найдена запись");
                    }
                    word.Name = basketEditDto.Name;
                    word.LanguageId = basketEditDto.LanguageId;
                    unitOfWork.Word.Update(word);
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<Word>.Success(word);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Word>.Failure().AddError(ex.Message);
                }
            }
        }
        public override List<WordDto> GetAllFilter(WordFilterDto filter)
        {
            if (filter == null)
                return new List<WordDto>();
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordList = unitOfWork.Word.GetAllFilter(filter.Name, filter.LanguageName);
                if (wordList.Count == 0) return new List<WordDto>();

                // Можно обойтись и без проверки на Count == 0, а сразу направить объект в маппер. Тогда он сгенерит пустую коллекцию
                return _mapper.Map<List<WordDto>>(wordList);
            }
        }

        public override List<WordDto> GetAllOfPageFilter(WordFilterDto filter, int pageNumber, int rowCount)
        {
            if (filter == null)
                return new List<WordDto>();
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordList = unitOfWork.Word.GetAllOfPageFilter(pageNumber, rowCount, filter.Name, filter.LanguageName);
                if (wordList.Count == 0) return new List<WordDto>();
                return _mapper.Map<List<WordDto>>(wordList);
            }
        }

        public List<WordDto> GetAllWordsForLanguage(int languageId)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordList = unitOfWork.Word.GetWordsForLanguage(languageId);
                if (wordList.Count == 0) return new List<WordDto>();
                return _mapper.Map<List<WordDto>>(wordList);
            }
        }
        
        public override List<WordDto> GetAllOfPage(int pageNumber, int rowCount)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<Word> wordList = unitOfWork.Word.GetAllOfPage(pageNumber, rowCount);
                if (wordList == null || wordList.Count == 0)
                {
                    return new List<WordDto>();
                }
                List<WordDto> retList = _mapper.Map<List<WordDto>>(wordList);
                return retList;
            }
        }

        public override List<WordDto> GetAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<Word> wordList = unitOfWork.Word.GetAll();
                if (wordList == null || wordList.Count == 0)
                {
                    return new List<WordDto>();
                }
                List<WordDto> retList = _mapper.Map<List<WordDto>>(wordList);
                return retList;
            }
        }

        public override WordDto GetByID(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                Word value = unitOfWork.GetRepository<Word>().GetById(id);
                if (value == null) return new WordDto();
                WordDto dto = _mapper.Map<WordDto>(value);
                return dto;

            }
        }

        protected override string CheckAndGetErrors(WordDto value, bool isNew = true)
        {
            string errors = string.Empty;
            if (string.IsNullOrEmpty(value.Name.Trim()))
                errors += "Не заполнено имя";
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                Language language = unitOfWork.Language.GetById(value.LanguageId);
                if (language == null)
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Текущий язык не найден в базе";
                }
                if (string.IsNullOrEmpty(errors) && unitOfWork.Word.IsNameReplay(value.Id, value.Name, value.LanguageId, isNew))
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Текое слово уже есть в базе";
                }
            }
            return errors;
        }

        protected override string CkeckBefforDelet(Word value)
        {
            string error = string.Empty;
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<WordTranslation> wordTranslations = unitOfWork.WordTranslation.GetWordTranslationsForWord(value.Id);
                if (wordTranslations?.Count > 0)
                {
                    error = "Слово " + value.Name + " имеет перевод:";
                    for (int i = 0; i < wordTranslations.Count; i++)
                    {
                        error += Environment.NewLine;
                        if (value.Id == wordTranslations[i].WordSource.Id)
                        {
                            error += "с языка " + wordTranslations[i].LanguageToWord.Name + "  " + wordTranslations[i].WordTranslationValue.Name;
                        }
                        else
                        {
                            error += "с языка " + wordTranslations[i].LanguageFromWord.Name + "  " + wordTranslations[i].WordSource.Name;
                        }
                    }
                }
            }
            return error;
        }

        protected override string CkeckBefforDeleteList(List<Word> listVal)
        {
            StringBuilder error = new StringBuilder(string.Empty);
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<int> idList = listVal.Select(x => x.Id).ToList();
                List<WordTranslation> wordTranslations = unitOfWork.WordTranslation.GetWordTranslationsForWord(idList);
                List<WordTranslation> wordTranslationsTemp = new List<WordTranslation>();
                if (wordTranslations?.Count > 0)
                {
                    for (int j = 0; j < listVal.Count; j++)
                    {
                        wordTranslationsTemp = wordTranslations.Where(x => x.WordSourceId == listVal[j].Id || x.WordTranslationId == listVal[j].Id).ToList();
                        if (wordTranslationsTemp == null || wordTranslationsTemp.Count == 0) continue;
                        if (j != 0) error.Append(Environment.NewLine);
                        error.Append("Слово " + listVal[j].Name + " имеет перевод:");
                        for (int i = 0; i < wordTranslationsTemp.Count; i++)
                        {
                            error.Append(Environment.NewLine);
                            if (listVal[j].Id == wordTranslationsTemp[i].WordSource.Id)
                            {
                                error.Append("с языка " + wordTranslationsTemp[i].LanguageToWord.Name + "  " + wordTranslationsTemp[i].WordTranslationValue.Name);
                            }
                            else
                            {
                                error.Append("с языка " + wordTranslationsTemp[i].LanguageFromWord.Name + "  " + wordTranslationsTemp[i].WordSource.Name);
                            }
                        }
                    }
                }
            }
            return error.ToString();
        }
    }
}
