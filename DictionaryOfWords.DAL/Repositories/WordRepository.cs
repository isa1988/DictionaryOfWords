﻿using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DictionaryOfWords.DAL.Repositories
{
    class WordRepository : RepositoryBase<Word>, IWordRepository
    {
        public WordRepository(DbContextDictionaryOfWords contextDictionaryOfWords) : base(contextDictionaryOfWords)
        {
            _dbSet = contextDictionaryOfWords.Words;
            IQueryable<Word> includeWord = GetInclude();
            base.DbSetInclude = includeWord;
        }

        public bool IsNameReplay(int id, string name, int languageId, bool isNew)
        {
            // Судя по алгоритму, у тебя не может быть двух одинаковых комбинаций name + languageId.
            // Если так, то смысла передавать сюда флаг isNew и айдишник id нет абсолютно, потому что не это избыточно.
            // Достаточно просто оставить поиск уже существующего слова по совпадению.
            // Причем вызывать такую проверку лучше только при добавлении нового слова. при апдейте ты уже себя обезопасил,
            // Что в базу не попадают дубликаты.
            // НУ а гарантией правильной работы метода вставки нового слова станут юниттесты

            // Забываешь о валидации. А вдруг в метод прислали null в качестве name?
            if (isNew)
            {
                return _dbSet.Any(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.LanguageId == languageId);
            }
            else
            {
                return _dbSet.Any(x => x.Id != id && x.Name.Trim().ToLower() == name.Trim().ToLower() && x.LanguageId == languageId);
            }
        }

        private IQueryable<Word> GetFilter(string name, string languageName)
        {
            IQueryable<Word> words = GetInclude();
            if (!string.IsNullOrWhiteSpace(name))
            {
                words = words.Where(x => EF.Functions.Like(x.Name, name.Like()));
            }
            if (!string.IsNullOrWhiteSpace(languageName))
            {
                words = words.Where(x => EF.Functions.Like(x.Language.Name, languageName.Like()));
            }
            return words;
        }

        public List<Word> GetAllOfPageFilter(int pageNumber, int rowCount, string name, string languageName)
        {
            int startIndex = (pageNumber - 1) * rowCount;
            var words = GetFilter(name, languageName)
                        .Skip(startIndex)
                        .Take(rowCount)
                        .ToList();

            return words;
        }

        // Везде и ниже лучше предпочесть асинхронные аналоги методов
        public List<Word> GetAllFilter(string name, string languageName)
        {
            var words = GetFilter(name, languageName)
                        .ToList();

            return words;
        }

        public List<Word> GetWordsForLanguage(int languageId)
        {
            return GetInclude().Where(x => x.LanguageId == languageId).ToList();
        }
        
        public List<Word> GetWordsForLanguage(List<int> languageIdList)
        {
            return GetInclude().Where(x => languageIdList.Any(n => x.LanguageId == n)).ToList();
        }

        public List<Word> GetWordsForTwoLanguage(List<string> words, int firstLanguageId, int secondLanguageId)
        {
            return GetInclude().Where(x => words.Any(n => n.ToLower() == x.Name.ToLower()) && 
                                          (x.LanguageId == firstLanguageId || x.LanguageId == secondLanguageId)).ToList();
        }

        public List<Word> GetWordsOfList(List<string> words, int languageId)
        {
            if (words == null || words.Count == 0) return new List<Word>();
            return _dbSet.Where(x => words.Any(n => n.ToLower() == x.Name.ToLower()) && x.LanguageId == languageId).ToList();
        }

        private IQueryable<Word> GetInclude()
        {
            return _dbSet.Include(x => x.Language);
        }

    }
}
