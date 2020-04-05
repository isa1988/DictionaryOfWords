using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DictionaryOfWords.DAL.Repositories
{
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
    {
        public LanguageRepository(DbContextDictionaryOfWords contextDictionaryOfWords) : base(contextDictionaryOfWords)
        {
            _dbSet = contextDictionaryOfWords.Languages;
        }

        public List<Language> GetLanguageListOfName(List<string> nameList)
        {
            if (nameList == null || nameList.Count == 0) return new List<Language>();
            return _dbSet.Where(x => nameList.Any(n => n.ToLower() == x.Name.ToLower())).ToList();
        }

        public bool IsNameReplay(int id, string name, bool isNew)
        {
            if (isNew)
            {
                return _dbSet.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
            }
            else
            {
                return _dbSet.Any(x => x.Id != id && x.Name.Trim().ToLower() == name.Trim().ToLower());
            }
        }

        private IQueryable<Language> GetFilter(string name)
        {
            IQueryable<Language> languages = _dbSet;
            if (!string.IsNullOrWhiteSpace(name))
            {
                languages = languages.Where(x => EF.Functions.Like(x.Name, name.Like()));
            }
            return languages;
        }

        public List<Language> GetAllOfPageFilter(int pageNumber, int rowCount, string name)
        {
            int startIndex = (pageNumber - 1) * rowCount;
            var languages = GetFilter(name)
                        .Skip(startIndex)
                        .Take(rowCount)
                        .ToList();

            return languages;
        }

        public List<Language> GetAllFilter(string name)
        {
            var languages = GetFilter(name)
                        .ToList();

            return languages;
        }

    }
}
