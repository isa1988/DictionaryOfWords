using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;

namespace DictionaryOfWords.Core.Repositories
{
    public interface IWordTranslationRepository : IRepository<WordTranslation>
    {
        bool IsNameReplay(int wordId, int languaageFromId, int languaageToId);

        List<WordTranslation> GetLanguageListOfName(List<int> wordListId, List<int> languaageListFromId, List<int> languaageListToId);
    }
}
