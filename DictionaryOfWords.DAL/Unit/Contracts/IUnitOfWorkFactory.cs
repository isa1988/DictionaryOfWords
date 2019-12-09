using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL.Unit.Contracts
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork MakeUnitOfWork();
    }
}
