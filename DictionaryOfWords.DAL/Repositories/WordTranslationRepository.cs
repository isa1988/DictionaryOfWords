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
    class WordTranslationRepository : Repository<WordTranslation>, IWordTranslationRepository
    {
        public WordTranslationRepository(DbContextDictionaryOfWords contextDictionaryOfWords) : base(contextDictionaryOfWords)
        {
            DbSet = contextDictionaryOfWords.WordTranslations;
            IQueryable<WordTranslation> includeWord = GetInclude();
            base.DbSetInclude = includeWord;
        }

        public List<WordTranslation> GetLanguageListOfName(List<int> wordListId, int languaageFrom, int languaageTo)
        {
            return DbSet.Where(x => (wordListId.Any(n => x.WordSourceId == n && x.LanguageFromId == languaageFrom && x.LanguageToId == languaageTo)) ||
                                  (wordListId.Any(n => x.WordTranslationId == n && x.LanguageToId == languaageFrom && x.LanguageFromId == languaageTo))).ToList();
        }

        public List<WordTranslation> GetWordTranslationsForWord(int wordId)
        {
            return GetInclude().Where(x => x.WordSourceId == wordId || x.WordTranslationId == wordId).ToList();
        }
        public List<WordTranslation> GetWordTranslationsForWord(List<int> wordIdList)
        {
            return GetInclude().Where(x => wordIdList.Any(n => x.WordSourceId == n || x.WordTranslationId == n)).ToList();
        }

        public bool IsNameReplay(int id, int wordId, int wordToId, int languaageFromId, int languaageToId, bool isNew)
        {
            if (isNew)
            {
                return DbSet.Any(x => (x.WordSourceId == wordId && x.WordTranslationId == wordToId && x.LanguageFromId == languaageFromId && x.LanguageToId == languaageToId) ||
                                      (x.WordTranslationId == wordId && x.WordSourceId == wordToId && x.LanguageToId == languaageFromId && x.LanguageFromId == languaageToId));
            }
            else
            {
                return DbSet.Any(x => (x.Id != id && x.WordSourceId == wordId && x.WordTranslationId == wordToId && x.LanguageFromId == languaageFromId && x.LanguageToId == languaageToId) ||
                                      (x.Id != id && x.WordTranslationId == wordId && x.WordSourceId == wordToId && x.LanguageToId == languaageFromId && x.LanguageFromId == languaageToId));
            }
        }

        private IQueryable<WordTranslation> GetInclude()
        {
            return DbSet.Include(x => x.LanguageFromWord).Include(x => x.WordSource).Include(x => x.LanguageToWord).Include(x => x.WordTranslationValue);
        }
    }

}
