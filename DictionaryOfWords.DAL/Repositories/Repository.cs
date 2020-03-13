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
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected DbContextDictionaryOfWords ContextDictionaryOfWords;
        protected DbSet<T> DbSet { get; set; }

        protected IQueryable<T> DbSetInclude { get; set; }

        public Repository(DbContextDictionaryOfWords contextDictionaryOfWords)
        {
            ContextDictionaryOfWords = contextDictionaryOfWords;
            DbSet = ContextDictionaryOfWords.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            var entry = await DbSet.AddAsync(entity);

            return entry.Entity;
        }
        private IQueryable<T> GetInclude()
        {
            return DbSetInclude != null ? DbSetInclude : DbSet;
        }

        public virtual List<T> GetAll()
        {
            return GetInclude().ToList();
        }
        //identity
        public async Task<List<T>> GetAllAsync()
        {
            return await GetInclude().ToListAsync();
        }

        public virtual T GetById(int id)
        {
            return GetInclude().FirstOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
