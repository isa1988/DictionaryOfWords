using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;

namespace DictionaryOfWords.DAL.Unit.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        ILanguageRepository Language { get; }
        IWordRepository Word { get; }
        IWordTranslationRepository WordTranslation { get; }

        Task<int> CompleteAsync();
        void BeginTransaction();
        //void BeginTransaction(IsolationLevel level);
        void RollbackTransaction();
        void CommitTransaction();

        IRepository<T> GetRepository<T>() where T : class, IEntity;
        IRepository<T, TId> GetRepository<T, TId>()
            where T : class, IEntity<TId>
            where TId : IEquatable<TId>;
    }
}
