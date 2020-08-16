using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;

namespace DictionaryOfWords.Core.Repositories
{
    public interface IWordRepository : IRepository<Word, int>
    {
        bool IsNameReplay(int id, string name, int languageId, bool isNew);

        List<Word> GetWordsForLanguage(int languageId);
        List<Word> GetWordsForLanguage(List<int> languageIdList);
        List<Word> GetWordsForTwoLanguage(List<string> words, int firstLanguageId, int secondLanguageId);

        List<Word> GetWordsOfList(List<string> words, int languageId);

        List<Word> GetAllFilter(string name, string languageName);
        List<Word> GetAllOfPageFilter(int pageNumber, int rowCount, string name, string languageName);
    }
}
