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
    class WordTranslationRepository : RepositoryBase<WordTranslation, int>, IWordTranslationRepository
    {
        public WordTranslationRepository(DbContextDictionaryOfWords contextDictionaryOfWords) : base(contextDictionaryOfWords)
        {
            _dbSet = contextDictionaryOfWords.WordTranslations;
            IQueryable<WordTranslation> includeWord = GetInclude();
            base.DbSetInclude = includeWord;
        }

        public List<WordTranslation> GetLanguageListOfName(List<int> wordListId, int languaageFrom, int languaageTo)
        {
            return _dbSet.Where(x => (wordListId.Any(n => x.WordSourceId == n && x.LanguageFromId == languaageFrom && x.LanguageToId == languaageTo)) ||
                                  (wordListId.Any(n => x.WordTranslationId == n && x.LanguageToId == languaageFrom && x.LanguageFromId == languaageTo))).ToList();
        }

        public List<WordTranslation> GetWordTranslationsForWord(int wordId)
        {
            return GetInclude().Where(x => x.WordSourceId == wordId || x.WordTranslationId == wordId).ToList();
        }
        public List<WordTranslation> GetWordTranslationsForWord(List<int> wordIdList)
        {
            return GetInclude().Where(x => wordIdList.Any(n => x.WordSourceId == n || x.WordTranslationId == n)).ToList();
        }

        public bool IsNameReplay(int id, int wordId, int wordToId, int languaageFromId, int languaageToId, bool isNew)
        {
            if (isNew)
            {
                return _dbSet.Any(x => (x.WordSourceId == wordId && x.WordTranslationId == wordToId && x.LanguageFromId == languaageFromId && x.LanguageToId == languaageToId) ||
                                      (x.WordTranslationId == wordId && x.WordSourceId == wordToId && x.LanguageToId == languaageFromId && x.LanguageFromId == languaageToId));
            }
            else
            {
                return _dbSet.Any(x => (x.Id != id && x.WordSourceId == wordId && x.WordTranslationId == wordToId && x.LanguageFromId == languaageFromId && x.LanguageToId == languaageToId) ||
                                      (x.Id != id && x.WordTranslationId == wordId && x.WordSourceId == wordToId && x.LanguageToId == languaageFromId && x.LanguageFromId == languaageToId));
            }
        }

        private IQueryable<WordTranslation> GetFilter(string wordFrom, string languageFrom, string wordTo, string languageTo)
        {
            IQueryable<WordTranslation> wordTranslations = GetInclude();
            if (!string.IsNullOrWhiteSpace(wordFrom))
            {
                wordTranslations = wordTranslations.Where(x => EF.Functions.Like(x.WordSource.Name, wordFrom.Like()) || EF.Functions.Like(x.WordTranslationValue.Name, wordFrom.Like()));
            }

            if (!string.IsNullOrWhiteSpace(languageFrom))
            {
                wordTranslations = wordTranslations.Where(x => EF.Functions.Like(x.LanguageFromWord.Name, languageFrom.Like()) || EF.Functions.Like(x.LanguageToWord.Name, languageFrom.Like()));
            }

            if (!string.IsNullOrWhiteSpace(wordTo))
            {
                wordTranslations = wordTranslations.Where(x => EF.Functions.Like(x.WordTranslationValue.Name, wordTo.Like()) || EF.Functions.Like(x.WordSource.Name, wordTo.Like()));
            }

            if (!string.IsNullOrWhiteSpace(languageTo))
            {
                wordTranslations = wordTranslations.Where(x => EF.Functions.Like(x.LanguageToWord.Name, languageTo.Like()) || EF.Functions.Like(x.LanguageFromWord.Name, languageTo.Like()));
            }

            return wordTranslations;
        }

        public List<WordTranslation> GetAllOfPageFilter(int pageNumber, int rowCount, string wordFrom, string languageFrom, string wordTo, string languageTo)
        {
            int startIndex = (pageNumber - 1) * rowCount;
            var wordTranslations = GetFilter(wordFrom, languageFrom, wordTo, languageTo)
                                    .Skip(startIndex)
                                    .Take(rowCount)
                                    .ToList();

            return wordTranslations;
        }

        public List<WordTranslation> GetAllFilter(string wordFrom, string languageFrom, string wordTo, string languageTo)
        {
            var wordTranslations = GetFilter(wordFrom, languageFrom, wordTo, languageTo)
                                   .ToList();

            return wordTranslations;
        }

        private IQueryable<WordTranslation> GetInclude()
        {
            return _dbSet.Include(x => x.LanguageFromWord).Include(x => x.WordSource).Include(x => x.LanguageToWord).Include(x => x.WordTranslationValue);
        }
    }

}
