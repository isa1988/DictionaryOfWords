using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.Core.DataBase;

namespace DictionaryOfWords.Core.Repositories
{
    public interface ILanguageRepository : IRepositoryBase<Language>
    {
        bool IsNameReplay(int id, string name, bool isNew);

        List<Language> GetLanguageListOfName(List<string> nameList);

        List<Language> GetAllFilter(string name);
        List<Language> GetAllOfPageFilter(int pageNumber, int rowCount, string name);
    }
}
