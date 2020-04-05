using DictionaryOfWords.DAL.Data.Contracts;
using DictionaryOfWords.DAL.Unit.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL.Unit
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWorkFactory(IDbContextFactory applicationDbContextFactory)
        {
            _applicationDbContextFactory = applicationDbContextFactory;
        }
        private readonly IDbContextFactory _applicationDbContextFactory;


        public IUnitOfWork MakeUnitOfWork()
        {
            return new UnitOfWork(_applicationDbContextFactory.Create());
        }
    }
}
