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
            return DbSetInclude != null ? DbSetInclude : _dbSet;
        }

        public virtual List<T> GetAll()
        {
            return GetInclude().ToList();
        }

        public virtual List<T> GetAllOfPage(int pageNumber, int rowCount)
        {
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
            return await GetInclude().Where(x => idList.Any(n => n == x.Id)).ToListAsync();
        }

        public virtual T GetById(int id)
        {
            return GetInclude().FirstOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteALot(List<T> entityList)
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                _dbSet.Remove(entityList[i]);
            }
        }
    }
}
