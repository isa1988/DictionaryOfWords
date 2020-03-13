using DictionaryOfWords.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IGeneralServiceDto<TBase, TDto> where TBase : Entity
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
    }
}
