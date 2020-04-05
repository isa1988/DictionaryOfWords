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

        IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;
    }
}
