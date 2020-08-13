﻿using AutoMapper;
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
        where TBase : EntityBase
        where TFilter : FilterBaseDto
    {
        public GeneralService(IUnitOfWorkFactory unitOfWorkFactory, TDto empty, IMapper mapper)
        {
            if (empty == null)
                throw new ArgumentNullException(nameof(empty));

            _unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
            _dtoEmpty = empty;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly TDto _dtoEmpty;
        protected readonly IMapper _mapper;


        protected abstract string CheckAndGetErrors(TDto value, bool isNew = true);

        public async Task<EntityOperationResult<TBase>> CreateItemAsync(TDto basketCreateDto)
        {
            // Валидацию модели можно сделать через атрибуты валидации, которые ты вешаешь на свойства класса.
            // Взывать проверку на основе атрибутов можно своим кодом. Я уже такой писал:
            // https://github.com/maximgorbatyuk/net-blank-app/blob/master/src/Utils/Validators/EntityValidator.cs
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
                    // Если ты хочешь использовать подход с классом-прокси EntityOperationResult,
                    // То я бы сохранял и передавал выше еще и сам объект ex, потому что в эксепшене важно не только месседж,
                    // но и стектрейс, и дополнительная инфа, и тип класса исключения.
                    // Но я бы отказался от такого прокси-класса и пробрасывал бы исключение наверх дальше, даже
                    // не отлавливая его здесь.
                    // Исключения ты отлавливаешь в одной точке - в созданной специально для этого middleware.
                    // а узнать, что это такое, ты можешь либо из нашего курса, либо на сайте metanit.com
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

        public virtual TDto GetByID(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                TBase value = unitOfWork.GetRepository<TBase>().GetById(id);
                if (value == null) return _dtoEmpty;
                TDto dto = _mapper.Map<TDto>(value);
                return dto;

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
}
