using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
using Microsoft.EntityFrameworkCore.Internal;

namespace DictionaryOfWords.DAL.Repositories
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(DbContextDictionaryOfWords contextDictionaryOfWords) : base(contextDictionaryOfWords)
        {
            DbSet = contextDictionaryOfWords.Languages;
        }

        public bool IsNameReplay(string name)
        {
            return DbSet.Any(x => x.Name.ToLower() == name.ToLower());
        }

        public override IEnumerable<Language> GetAll()
        {
            return DbSet.ToList();
        }

        public override Language GetById(int id)
        {
            return DbSet.FirstOrDefault(p => p.Id == id);
        }
    }
}
