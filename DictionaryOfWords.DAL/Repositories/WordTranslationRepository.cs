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
        }

        private IQueryable<WordTranslation> GetInclude()
        {
            return DbSet.Include(x => x.LanguageFromWord).Include(x => x.WordSource).Include(x => x.LanguageToWord).Include(x => x.WordTranslationValue);
        }
    }
}
