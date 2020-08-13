using DictionaryOfWords.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Core.Repositories
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<T> AddAsync(T entity);

        // Мешать вместе синхронный и асинхронный интерфейс не стоит - это конфьюзит клиентов твоего кода.
        // Да и в целом в .NET COre нужно предпочитать асинхронный интерфейс библиотек и самому писать такой.
        List<T> GetAll();
        List<T> GetAllOfPage(int pageNumber, int rowCount);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllOfIdAsync(List<int> idList);
        T GetById(int id);
        void Update(T entity);
        void Delete(T entity);

        // Можно и перегрузкой вполне сделать метод
        // void Delete(List<T> entities);
        void DeleteALot(List<T> entity);
    }
}
