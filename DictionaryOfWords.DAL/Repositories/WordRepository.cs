using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
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
        }

        public bool IsNameReplay(string name)
        {
            return DbSet.Any(x => x.Name.ToLower() == name.ToLower());
        }

        public override IEnumerable<Word> GetAll()
        {
            return DbSet.ToList();
        }

        public override Word GetById(int id)
        {
            return DbSet.FirstOrDefault(p => p.Id == id);
        }
    }
}
