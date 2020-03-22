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

        public List<Language> GetLanguageListOfName(List<string> nameList)
        {
            if (nameList == null || nameList.Count == 0) return new List<Language>();
            return DbSet.Where(x => nameList.Any(n => n.ToLower() == x.Name.ToLower())).ToList();
        }

        public bool IsNameReplay(int id, string name, bool isNew)
        {
            if (isNew)
            {
                return DbSet.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
            }
            else
            {
                return DbSet.Any(x => x.Id != id && x.Name.Trim().ToLower() == name.Trim().ToLower());
            }
        }

    }
}
