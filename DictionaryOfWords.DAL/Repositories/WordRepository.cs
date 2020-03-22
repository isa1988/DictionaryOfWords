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
            DbSet = contextDictionaryOfWords.Words;
            IQueryable<Word> includeWord = GetInclude();
            base.DbSetInclude = includeWord;
        }

        public bool IsNameReplay(int id, string name, int languageId, bool isNew)
        {
            if (isNew)
            {
                return DbSet.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.LanguageId == languageId);
            }
            else
            {
                return DbSet.Any(x => x.Id != id && x.Name.Trim().ToLower() == name.Trim().ToLower() && x.LanguageId == languageId);
            }
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
            return DbSet.Where(x => words.Any(n => n.ToLower() == x.Name.ToLower()) && x.LanguageId == languageId).ToList();
        }

        private IQueryable<Word> GetInclude()
        {
            return DbSet.Include(x => x.Language);
        }

    }
}
