using DictionaryOfWords.DAL.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContextOptions<DbContextDictionaryOfWords> _options;

        public DbContextFactory(
            DbContextOptions<DbContextDictionaryOfWords> options)
        {
            _options = options;
        }

        public DbContextDictionaryOfWords Create()
        {
            return new DbContextDictionaryOfWords(_options);
        }
    }
}
