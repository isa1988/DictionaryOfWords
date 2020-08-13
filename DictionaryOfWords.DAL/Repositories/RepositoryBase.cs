using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.DAL.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        public RepositoryBase(DbContextDictionaryOfWords contextDictionaryOfWords)
        {
            _contextDictionaryOfWords = contextDictionaryOfWords;
            _dbSet = _contextDictionaryOfWords.Set<T>();
        }
        protected DbContextDictionaryOfWords _contextDictionaryOfWords;
        protected DbSet<T> _dbSet { get; set; }

        protected IQueryable<T> DbSetInclude { get; set; }


        public async Task<T> AddAsync(T entity)
        {
            var entry = await _dbSet.AddAsync(entity);

            return entry.Entity;
        }
        private IQueryable<T> GetInclude()
        {
            // Если ты хочешь, чтобы клиенты твоего репозитория могли инклюдить в каждом репозитории свои нужные сущности,
            // А результат использовать в этом классе, то лучше создать свойство DbSetInclude либо асбтрактным, либо виртуальным, чтобы
            // эти самые клиенты не инициировали эту переменную DbSetInclude так. как делают сейчас.
            // Получается, что если кто-то зафакапит иниициацию в своем репозитории этой переменной, то он всегда
            // будет работать с незаинклюженным сетом.
            // А вот необходимость реализовать абстрактный метод вынудит его это сделать со 100%-ной гарантией
            return DbSetInclude != null ? DbSetInclude : _dbSet;
        }

        public virtual List<T> GetAll()
        {
            return GetInclude().ToList();
        }

        public virtual List<T> GetAllOfPage(int pageNumber, int rowCount)
        {
            // Снова код пагинации. Экстеншн метод
            int startIndex = (pageNumber - 1) * rowCount;
            return GetInclude()
                   .Skip(startIndex)
                   .Take(rowCount)
                   .ToList();
        }
        //identity
        public async Task<List<T>> GetAllAsync()
        {
            return await GetInclude().ToListAsync();
        }

        public async Task<List<T>> GetAllOfIdAsync(List<int> idList)
        {
            // Лучше применить метод idList.Contains(x.Id),
            // тогда EF провайдер сделать SQL команду с контейнсом. Здесь же я подозреваю, что
            // создается выборка на всю таблицу, а уже в памяти приоржения происходить определение, какие строки вернуть
            return await GetInclude().Where(x => idList.Any(n => n == x.Id)).ToListAsync();
        }

        public virtual T GetById(int id)
        {
            return GetInclude().FirstOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {
            // А как же сохранение контекста SaveChanges?
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            // А как же сохранение контекста SaveChanges?
            _dbSet.Remove(entity);
        }

        public void DeleteALot(List<T> entityList)
        {
            // Можно проще
            _dbSet.RemoveRange(entityList);
        }
    }
}
