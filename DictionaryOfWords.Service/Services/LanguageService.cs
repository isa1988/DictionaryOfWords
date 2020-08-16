using AutoMapper;
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
    public class LanguageService : GeneralServiceWithId<Language, LanguageDto, LanguageFilterDto>, ILanguageService
    {
        public LanguageService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, new LanguageDto(), mapper)
        {
        }


        public async Task<EntityOperationResult<Language>> EditItemAsync(LanguageDto basketEditDto)
        {
            string errors = CheckAndGetErrors(basketEditDto, false);
            if (!string.IsNullOrEmpty(errors))
            {
                return EntityOperationResult<Language>.Failure().AddError(errors);
            }
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    var language = unitOfWork.Language.GetById(basketEditDto.Id);
                    if (language == null)
                    {
                        return EntityOperationResult<Language>.Failure().AddError("Не найдена запись");
                    }
                    language.Name = basketEditDto.Name;
                    unitOfWork.Language.Update(language);
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<Language>.Success(language);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<Language>.Failure().AddError(ex.Message);
                }
            }
        }

        protected override string CheckAndGetErrors(LanguageDto value, bool isNew = true)
        {
            string errors = string.Empty;
            if (string.IsNullOrEmpty(value.Name.Trim()))
                errors += "Не заполнено имя";
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (string.IsNullOrEmpty(errors) && unitOfWork.Language.IsNameReplay(value.Id, value.Name, isNew))
                {
                    errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                    errors += "Текущий язык уже есть в базе";
                }
            }
            return errors;
        }
        protected override string CkeckBefforDelet(Language value)
        {
            string error = string.Empty;
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<Word> words = unitOfWork.Word.GetWordsForLanguage(value.Id);
                if (words?.Count > 0)
                {
                    error = "Язык " + value.Name + " имеет слова:";
                    for (int i = 0; i < words.Count; i++)
                    {
                        error += Environment.NewLine;
                        error += words[i].Name;
                    }
                }
            }
            return error;
        }

        protected override string CkeckBefforDeleteList(List<Language> listVal)
        {
            StringBuilder error = new StringBuilder(string.Empty);
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<int> idList = listVal.Select(x => x.Id).ToList();
                List<Word> words = unitOfWork.Word.GetWordsForLanguage(idList);
                List<Word> wordsTemp = new List<Word>();
                if (words?.Count > 0)
                {
                    for (int j = 0; j < listVal.Count; j++)
                    {
                        wordsTemp = words.Where(x => x.LanguageId == listVal[j].Id).ToList();
                        if (wordsTemp == null || wordsTemp.Count == 0) continue;
                        if (j == 0) error.Append(Environment.NewLine);
                        error.Append("Язык " + listVal[j].Name + " имеет слова:");
                        for (int i = 0; i < wordsTemp.Count; i++)
                        {
                            error.Append(Environment.NewLine);
                            error.Append(wordsTemp[i].Name);
                        }
                    }
                }
            }
            return error.ToString();
        }

        public override List<LanguageDto> GetAllFilter(LanguageFilterDto filter)
        {
            if (filter == null)
                return new List<LanguageDto>();
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var languageList = unitOfWork.Language.GetAllFilter(filter.Name);
                if (languageList.Count == 0) return new List<LanguageDto>();
                return _mapper.Map<List<LanguageDto>>(languageList);
            }
        }
        
        public override List<LanguageDto> GetAllOfPageFilter(LanguageFilterDto filter, int pageNumber, int rowCount)
        {
            if (filter == null)
                return new List<LanguageDto>();
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                var languageList = unitOfWork.Language.GetAllOfPageFilter(pageNumber, rowCount, filter.Name);
                if (languageList.Count == 0) return new List<LanguageDto>();
                return _mapper.Map<List<LanguageDto>>(languageList);
            }
        }
    }
}
