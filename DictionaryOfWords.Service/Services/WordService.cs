using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services
{
    public class WordService : GeneralServiceDto<Word, WordDto>, IWordService
    {
        public WordService(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory, new WordDto())
        {
        }

        public override string CheckAndGetErrors(WordDto value, bool isNew = true)
        {
            string errors = string.Empty;
            if (string.IsNullOrEmpty(value.Name))
                errors += "Не заполнено имя";
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                Language language = unitOfWork.Language.GetById(value.LanguageId);
                if (language == null)
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Текущий язык не найден в базе";
                }
                if (!isNew)
                {

                    if (unitOfWork.Word.IsNameReplay(value.Name, value.LanguageId))
                    {
                        errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                        errors += "Текое слово уже есть в базе";
                    }
                }
            }
            return errors;
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

        public List<WordDto> GetAllWordsForLanguage(int languageId)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var wordList = unitOfWork.Word.GetWordsForLanguage(languageId);
                if (wordList.Count == 0) return new List<WordDto>();
                return AutoMapper.Mapper.Map<List<WordDto>>(wordList);
            }
        }

        public List<LanguageDto> GetLanguageList()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var languageList = unitOfWork.Language.GetAll();
                if (languageList.Count == 0) return new List<LanguageDto>();
                return AutoMapper.Mapper.Map<List<LanguageDto>>(languageList);
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
                List<WordDto> retList = Mapper.Map<List<WordDto>>(wordList);
                return retList;
            }
        }

        public WordDto GetByID(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                Word value = unitOfWork.GetRepository<Word>().GetById(id);
                if (value == null) return new WordDto();
                WordDto dto = Mapper.Map<WordDto>(value);
                return dto;

            }
        }
    }
}
