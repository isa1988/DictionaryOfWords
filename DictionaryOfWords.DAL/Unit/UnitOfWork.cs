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
        private readonly DbContextDictionaryOfWords _contextDictionaryOfWords;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        private IDbContextTransaction _transaction;

        private bool _disposed;

        public UnitOfWork(DbContextDictionaryOfWords contextDictionaryOfWords)
        {
            _contextDictionaryOfWords = contextDictionaryOfWords;
            _repositories = new ConcurrentDictionary<Type, object>();

            Language = new LanguageRepository(contextDictionaryOfWords);
            Word = new WordRepository(contextDictionaryOfWords);
            WordTranslation = new WordTranslationRepository(contextDictionaryOfWords);
        }

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

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            return _repositories.GetOrAdd(typeof(TEntity), (object)new Repository<TEntity>(_contextDictionaryOfWords)) as IRepository<TEntity>;
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
