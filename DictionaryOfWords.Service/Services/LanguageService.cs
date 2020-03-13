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
    public class LanguageService : GeneralServiceDto<Language, LanguageDto>, ILanguageService
    {
        public LanguageService(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory, new LanguageDto())
        {

        }

        public override string CheckAndGetErrors(LanguageDto value, bool isNew = true)
        {
            string errors = string.Empty;
            if (string.IsNullOrEmpty(value.Name))
                errors += "Не заполнено имя";
            if (!isNew)
            {
                using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
                {
                    if (unitOfWork.Language.IsNameReplay(value.Name))
                    {
                        errors += errors.Length > 0 ? Environment.NewLine : string.Empty;
                        errors += "Текущий язык уже есть в базе";
                    }
                }
            }
            return errors;
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
                    //language = Mapper.Map<Language>(basketEditDto);
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
    }
}
