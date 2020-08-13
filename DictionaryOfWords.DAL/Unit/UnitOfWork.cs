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

    // Я лично не очень разделяю радость от использования паттерна UnitOfWork, потому что считаю,
    // что сам DatabaseContext уже и есть UoF,
    // поэтому создавать такую же абстракцию над ним не нужно. Но если ты хочешь, то ок.
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DbContextDictionaryOfWords contextDictionaryOfWords)
        {
            _contextDictionaryOfWords = contextDictionaryOfWords;
            _repositories = new ConcurrentDictionary<Type, object>();

            // Лучше было бы инъектить их через конструктор из DI.
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
            // Если кто-то вызвал в своем коде этот метод CommitTransaction() без предварительного вызова BeginTransaction(),
            // то лучше ему это показать, выбросив эксепшн. Пусть клиенты твоео кода учатся работать с ним.
            // Так они быстро поймут, как работать с кодом.
            // Также они могли просто забыть вызвать где-то в своих методах инициацию транзакции,
            // а ты своим эксепшеном указал на это.
            // Сейчас же ты игноришь проблему просто, и это может привести к неожиданному поведению системы,
            // когда подьзователь заполняет форму создания какой-то модели, жмет на кнопку "сохранить",
            // не видит ни одной ошибки сайта, но модель не создана.
            if (_transaction == null) return;

            _transaction.Commit();
            _transaction.Dispose();

            _transaction = null;
        }

        public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        {
            return _repositories.GetOrAdd(typeof(TEntity), (object)new RepositoryBase<TEntity>(_contextDictionaryOfWords)) as IRepositoryBase<TEntity>;
        }

        public void RollbackTransaction()
        {
            // См коммент к методу public void CommitTransaction()
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
