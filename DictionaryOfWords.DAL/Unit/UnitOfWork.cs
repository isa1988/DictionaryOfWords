using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
using DictionaryOfWords.DAL.Repositories;
using DictionaryOfWords.DAL.Unit.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.DAL.Unit
{

    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DbContextDictionaryOfWords contextDictionaryOfWords)
        {
            _contextDictionaryOfWords = contextDictionaryOfWords;
            _repositories = new ConcurrentDictionary<Type, object>();

            Language = new LanguageRepository(contextDictionaryOfWords);
            Word = new WordRepository(contextDictionaryOfWords);
            WordTranslation = new WordTranslationRepository(contextDictionaryOfWords);
        }

        private readonly DbContextDictionaryOfWords _contextDictionaryOfWords;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        private IDbContextTransaction _transaction;

        private bool _disposed;


        public ILanguageRepository Language { get; }
        public IWordRepository Word { get; }
        public IWordTranslationRepository WordTranslation { get; }
        

        public Task<int> CompleteAsync()
        {
            return _contextDictionaryOfWords.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _transaction = _contextDictionaryOfWords.Database.BeginTransaction();
        }

        //public void BeginTransaction(IsolationLevel level)
        //{
        //    _transaction = _contextDictionaryOfWords.Database.BeginTransaction(level);
        //}

        public void CommitTransaction()
        {
            if (_transaction == null) return;

            _transaction.Commit();
            _transaction.Dispose();

            _transaction = null;
        }

        public IRepository<T> GetRepository<T>() 
            where T : class, IEntity
        {
            return _repositories.GetOrAdd(typeof(T), (object)new RepositoryBase<T>(_contextDictionaryOfWords)) as IRepository<T>;
        }

        public IRepository<T, TId> GetRepository<T, TId>()
            where T : class, IEntity<TId>
            where TId : IEquatable<TId>
        {
            return _repositories.GetOrAdd(typeof(T), (object)new RepositoryBase<T, TId>(_contextDictionaryOfWords)) as IRepository<T, TId>;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();

            _transaction = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _contextDictionaryOfWords.Dispose();

            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
