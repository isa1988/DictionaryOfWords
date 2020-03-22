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

        protected abstract string CheckAndGetErrors(TDto value, bool isNew = true);

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

        protected abstract string CkeckBefforDelet(TBase value);

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
                    string error = CkeckBefforDelet(value);
                    if (!string.IsNullOrEmpty(error))
                    {
                        return EntityOperationResult<TBase>.Failure().AddError(error);
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

        protected abstract string CkeckBefforDeleteList(List<TBase> listVal);
        public async Task<EntityOperationResult<TBase>> DeleteItemAsync(List<int> idList)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    if (idList == null || idList.Count == 0)
                    {
                        return EntityOperationResult<TBase>.Failure().AddError("Не задан лист с id");
                    }
                    List<TBase> listVal = await unitOfWork.GetRepository<TBase>().GetAllOfIdAsync(idList);
                    if (listVal == null || listVal.Count == 0)
                    {
                        return EntityOperationResult<TBase>.Failure().AddError("Не найдена запись");
                    }
                    string error = CkeckBefforDeleteList(listVal);
                    if (!string.IsNullOrEmpty(error))
                    {
                        return EntityOperationResult<TBase>.Failure().AddError(error);
                    }
                    unitOfWork.GetRepository<TBase>().DeleteALot(listVal);
                    await unitOfWork.CompleteAsync();

                    return EntityOperationResult<TBase>.Success(listVal[0]);
                }
                catch (Exception ex)
                {
                    return EntityOperationResult<TBase>.Failure().AddError(ex.Message);
                }
            }
        }

        public virtual List<TDto> GetAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<TBase> valueBaseList = unitOfWork.GetRepository<TBase>().GetAll();
                if (valueBaseList == null || valueBaseList.Count == 0)
                {
                    return new List<TDto>();
                }
                List<TDto> retList = Mapper.Map<List<TDto>>(valueBaseList);
                return retList;
            }
        }

        public virtual TDto GetByID(int id)
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
