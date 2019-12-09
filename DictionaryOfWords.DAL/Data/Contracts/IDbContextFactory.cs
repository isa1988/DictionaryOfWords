using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL.Data.Contracts
{
    public interface IDbContextFactory
    {
        DbContextDictionaryOfWords Create();
    }
}
