using DictionaryOfWords.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Core.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T> AddAsync(T entity);
        List<T> GetAll();
        List<T> GetAllOfPage(int pageNumber, int rowCount);
        Task<List<T>> GetAllAsync();
        void Update(T entity);
        void Delete(T entity);
        void DeleteALot(List<T> entity);
    }

    public interface IRepository<T, TId> : IRepository<T>
        where T : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        Task<List<T>> GetAllOfIdAsync(List<TId> idList);
        T GetById(TId id);
    }
}
