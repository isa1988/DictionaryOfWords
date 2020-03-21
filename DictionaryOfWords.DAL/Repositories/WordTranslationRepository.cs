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

        public List<WordTranslation> GetLanguageListOfName(List<int> wordListId, List<int> languaageListFromId, List<int> languaageListToId)
        {
            if (wordListId.Count != languaageListFromId.Count || wordListId.Count != languaageListToId.Count ||
                languaageListFromId.Count != languaageListToId.Count) return new List<WordTranslation>();
            List<WordAndLanguageID> wordAndLanguages = new List<WordAndLanguageID>();
            for (int i = 0; i < wordListId.Count; i++)
            {
                wordAndLanguages.Add(new WordAndLanguageID(wordListId[i], languaageListFromId[i], languaageListToId[i]));
            }
            return DbSet.Where(x => (wordAndLanguages.Any(n => x.WordSourceId == n.WordId && x.LanguageFromId == n.LanguaageFromId && x.LanguageToId == n.LanguaageToId)) ||
                                  (wordAndLanguages.Any(n => x.WordTranslationId == n.WordId && x.LanguageToId == n.LanguaageFromId && x.LanguageFromId == n.LanguaageToId))).ToList();
        }

        public bool IsNameReplay(int wordId, int languaageFromId, int languaageToId)
        {
            return DbSet.Any(x => (x.WordSourceId == wordId && x.LanguageFromId == languaageFromId && x.LanguageToId == languaageToId) ||
                                  (x.WordTranslationId == wordId && x.LanguageToId == languaageFromId && x.LanguageFromId == languaageToId));
        }

        private IQueryable<WordTranslation> GetInclude()
        {
            return DbSet.Include(x => x.LanguageFromWord).Include(x => x.WordSource).Include(x => x.LanguageToWord).Include(x => x.WordTranslationValue);
        }
    }

    class WordAndLanguageID
    {
        public WordAndLanguageID(int wordId, int languaageFromId, int languaageToId)
        {
            WordId = wordId;
            LanguaageFromId = languaageFromId;
            LanguaageToId = languaageToId;
        }

        public int WordId { get; set; }
        public int LanguaageFromId { get; set; }
        public int LanguaageToId { get; set; }


    }
}
