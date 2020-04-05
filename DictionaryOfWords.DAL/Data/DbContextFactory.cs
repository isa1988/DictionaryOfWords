using DictionaryOfWords.DAL.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        public DbContextFactory(
            DbContextOptions<DbContextDictionaryOfWords> options)
        {
            _options = options;
        }

        private readonly DbContextOptions<DbContextDictionaryOfWords> _options;


        public DbContextDictionaryOfWords Create()
        {
            return new DbContextDictionaryOfWords(_options);
        }
    }
}
