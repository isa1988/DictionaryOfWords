using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services
{
    public abstract class GeneralServiceDto<TBase, TDto> : IGeneralServiceDto<TBase, TDto> where TBase : Entity
    {
        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly TDto _dtoEmpty;

        public GeneralServiceDto(IUnitOfWorkFactory unitOfWorkFactory, TDto empty)
        {
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException(nameof(unitOfWorkFactory));
            if (empty == null)
                throw new ArgumentNullException(nameof(empty));

            _unitOfWorkFactory = unitOfWorkFactory;
            _dtoEmpty = empty;
        }

        public abstract string CheckAndGetErrors(TDto value, bool isNew = true);

        public async Task<EntityOperationResult<TBase>> CreateItemAsync(TDto basketCreateDto)
        {
            string errors = CheckAndGetErrors(basketCreateDto);
            if (!string.IsNullOrEmpty(errors))
            {
                return EntityOperationResult<TBase>.Failure().AddError(errors);
            }
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {   
                try
                {
                    TBase value = Mapper.Map<TBase>(basketCreateDto);
                    var entity = await unitOfWork.GetRepository<TBase>().AddAsync(value);
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<TBase>.Success(entity);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<TBase>.Failure().AddError(ex.Message);
                }
            }
        }

        public async Task<EntityOperationResult<TBase>> DeleteItemAsync(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    TBase value = unitOfWork.GetRepository<TBase>().GetById(id);
                    if (value == null)
                    {
                        return EntityOperationResult<TBase>.Failure().AddError("Не найдена запись");
                    }
                    unitOfWork.GetRepository<TBase>().Delete(value);
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<TBase>.Success(value);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<TBase>.Failure().AddError(ex.Message);
                }
            }
        }

        public List<TDto> GetAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<TBase> valueBaseList = unitOfWork.GetRepository<TBase>().GetAll().ToList();
                if (valueBaseList == null || valueBaseList.Count == 0)
                {
                    return new List<TDto>();
                }
                List<TDto> retList = Mapper.Map<List<TDto>>(valueBaseList);
                return retList;
            }
        }

        public TDto GetByID(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                TBase value = unitOfWork.GetRepository<TBase>().GetById(id);
                if (value == null) return _dtoEmpty;
                TDto dto = Mapper.Map<TDto>(value);
                return dto;

            }
        }
    }
}
