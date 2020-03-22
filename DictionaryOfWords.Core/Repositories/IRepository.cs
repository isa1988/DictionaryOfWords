using DictionaryOfWords.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Core.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity);
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllOfIdAsync(List<int> idList);
        T GetById(int id);
        void Update(T entity);
        void Delete(T entity);
        void DeleteALot(List<T> entity);
    }
}
