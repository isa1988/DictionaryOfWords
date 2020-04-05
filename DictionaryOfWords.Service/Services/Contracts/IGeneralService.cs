using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos.FilterDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IGeneralService<TBase, TDto, TFilter> 
        where TBase : EntityBase
        where TFilter : FilterBaseDto
    {
        /// <summary>
        /// Добавить запись в базу
        /// </summary>
        /// <param name="basketCreateDto">Объект добавление</param>
        /// <returns></returns>
        Task<EntityOperationResult<TBase>> CreateItemAsync(TDto basketCreateDto);
        
        /// <summary>
        /// Удалить запись из базы
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        Task<EntityOperationResult<TBase>> DeleteItemAsync(int id);


        /// <summary>
        /// Удалить запись из базы несколько записей
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        Task<EntityOperationResult<TBase>> DeleteItemAsync(List<int> idList);

        /// <summary>
        /// Вернуть конкретный объект из базы
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        TDto GetByID(int id);

        /// <summary>
        /// Вернуть все записи
        /// </summary>
        /// <returns></returns>
        List<TDto> GetAll();


        /// <summary>
        /// Вернуть все записи
        /// </summary>
        /// <returns></returns>
        List<TDto> GetAllOfPage(int pageNumber, int rowCount);


        /// <summary>
        /// Вернуть все записи по фильтру
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <returns></returns>
        List<TDto> GetAllFilter(TFilter filter);

        /// <summary>
        /// Вернуть количество записей по списку
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <returns></returns>
        int GetCountOfAllFilter(TFilter filter);

        /// <summary>
        /// Постраничное отображение записей
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="pageNumber">страница</param>
        /// <param name="rowCount">записи на странице</param>
        /// <returns></returns>
        List<TDto> GetAllOfPageFilter(TFilter filter, int pageNumber, int rowCount);
    }
}
