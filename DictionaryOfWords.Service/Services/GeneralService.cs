using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos.FilterDto;
using DictionaryOfWords.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services
{
    public abstract class GeneralService<TBase, TDto, TFilter> : IGeneralService<TBase, TDto, TFilter> 
        where TBase : class, IEntity
        where TFilter : FilterBaseDto
    {
        public GeneralService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
        {
            if (unitOfWorkFactory == null)
                throw new ArgumentNullException(nameof(unitOfWorkFactory));
            
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _unitOfWorkFactory = unitOfWorkFactory;
            _mapper = mapper;
        }

        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;
        protected readonly IMapper _mapper;


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
                    TBase value = _mapper.Map<TBase>(basketCreateDto);
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
               
        public virtual List<TDto> GetAllOfPage(int pageNumber, int rowCount)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<TBase> valueBaseList = unitOfWork.GetRepository<TBase>().GetAllOfPage(pageNumber, rowCount);
                if (valueBaseList == null || valueBaseList.Count == 0)
                {
                    return new List<TDto>();
                }
                List<TDto> retList = _mapper.Map<List<TDto>>(valueBaseList);
                return retList;
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
                List<TDto> retList = _mapper.Map<List<TDto>>(valueBaseList);
                return retList;
            }
        }

        public abstract List<TDto> GetAllFilter(TFilter filter);

        public virtual int GetCountOfAllFilter(TFilter filter)
        {
            var retList = GetAllFilter(filter);
            return retList.Count;
        }

        public abstract List<TDto> GetAllOfPageFilter(TFilter filter, int pageNumber, int rowCount);
    }

    public abstract class GeneralServiceWithId<TBase, TDto, TFilter> : GeneralService<TBase, TDto, TFilter>,
        IGeneralServiceWithId<TBase, TDto, TFilter>
        where TBase : class, IEntity<int>
        where TFilter : FilterBaseDto
    {
        private readonly TDto _dtoEmpty;
        public GeneralServiceWithId(IUnitOfWorkFactory unitOfWorkFactory, TDto empty, IMapper mapper)
            : base(unitOfWorkFactory, mapper)
        {
            if (empty == null)
                throw new ArgumentNullException(nameof(empty));
            _dtoEmpty = empty;
        }

        protected abstract string CkeckBefforDelet(TBase value);

        public async Task<EntityOperationResult<TBase>> DeleteItemAsync(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                try
                {
                    TBase value = unitOfWork.GetRepository<TBase, int>().GetById(id);
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
                    List<TBase> listVal = await unitOfWork.GetRepository<TBase, int>().GetAllOfIdAsync(idList);
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

        public virtual TDto GetByID(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                TBase value = unitOfWork.GetRepository<TBase, int>().GetById(id);
                if (value == null) return _dtoEmpty;
                TDto dto = _mapper.Map<TDto>(value);
                return dto;

            }
        }
    }
}
