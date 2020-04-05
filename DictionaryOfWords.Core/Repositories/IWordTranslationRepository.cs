using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;

namespace DictionaryOfWords.Core.Repositories
{
    public interface IWordTranslationRepository : IRepositoryBase<WordTranslation>
    {
        bool IsNameReplay(int id, int wordId, int wordToId, int languaageFromId, int languaageToId, bool isNew);

        List<WordTranslation> GetLanguageListOfName(List<int> wordListId, int languaageFrom, int languaageTo);

        List<WordTranslation> GetWordTranslationsForWord(int wordId);
        List<WordTranslation> GetWordTranslationsForWord(List<int> wordIdList);
        
        List<WordTranslation> GetAllFilter(string wordFrom, string languageFrom, string wordTo, string languageTo);
        List<WordTranslation> GetAllOfPageFilter(int pageNumber, int rowCount, string wordFrom, string languageFrom, string wordTo, string languageTo);
    }
}
