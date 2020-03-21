using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;

namespace DictionaryOfWords.Core.Repositories
{
    public interface ILanguageRepository : IRepository<Language>
    {
        bool IsNameReplay(string name);

        List<Language> GetLanguageListOfName(List<string> nameList);
    }
}
