using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
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

        public override IEnumerable<WordTranslation> GetAll()
        {
            return DbSet.ToList();
        }

        public override WordTranslation GetById(int id)
        {
            return DbSet.FirstOrDefault(p => p.Id == id);
        }
    }
}
