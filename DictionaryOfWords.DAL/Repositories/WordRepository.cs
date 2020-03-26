using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DictionaryOfWords.DAL.Repositories
{
    class WordRepository : Repository<Word>, IWordRepository
    {
        public WordRepository(DbContextDictionaryOfWords contextDictionaryOfWords) : base(contextDictionaryOfWords)
        {
            _dbSet = contextDictionaryOfWords.Words;
            IQueryable<Word> includeWord = GetInclude();
            base.DbSetInclude = includeWord;
        }

        public bool IsNameReplay(int id, string name, int languageId, bool isNew)
        {
            if (isNew)
            {
                return _dbSet.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.LanguageId == languageId);
            }
            else
            {
                return _dbSet.Any(x => x.Id != id && x.Name.Trim().ToLower() == name.Trim().ToLower() && x.LanguageId == languageId);
            }
        }

        private IQueryable<Word> GetFilter(string name, string languageName)
        {
            IQueryable<Word> words = GetInclude();
            if (!string.IsNullOrWhiteSpace(name))
            {
                words = words.Where(x => x.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(languageName))
            {
                words = words.Where(x => x.Language.Name.Contains(languageName));
            }
            return words;
        }

        public List<Word> GetAllOfPageFilter(int pageNumber, int rowCount, string name, string languageName)
        {
            int startIndex = (pageNumber - 1) * rowCount;
            var words = GetFilter(name, languageName)
                        .Skip(startIndex)
                        .Take(rowCount)
                        .ToList();

            return words;
        }

        public List<Word> GetAllFilter(string name, string languageName)
        {
            var words = GetFilter(name, languageName)
                        .ToList();

            return words;
        }

        public List<Word> GetWordsForLanguage(int languageId)
        {
            return GetInclude().Where(x => x.LanguageId == languageId).ToList();
        }
        
        public List<Word> GetWordsForLanguage(List<int> languageIdList)
        {
            return GetInclude().Where(x => languageIdList.Any(n => x.LanguageId == n)).ToList();
        }

        public List<Word> GetWordsForTwoLanguage(List<string> words, int firstLanguageId, int secondLanguageId)
        {
            return GetInclude().Where(x => words.Any(n => n.ToLower() == x.Name.ToLower()) && 
                                          (x.LanguageId == firstLanguageId || x.LanguageId == secondLanguageId)).ToList();
        }

        public List<Word> GetWordsOfList(List<string> words, int languageId)
        {
            if (words == null || words.Count == 0) return new List<Word>();
            return _dbSet.Where(x => words.Any(n => n.ToLower() == x.Name.ToLower()) && x.LanguageId == languageId).ToList();
        }

        private IQueryable<Word> GetInclude()
        {
            return _dbSet.Include(x => x.Language);
        }

    }
}
