using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.DAL.Unit.Contracts;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.Service.Services
{
    public class MultiAddToBaseService : IMultiAddToBaseService
    {
        IUnitOfWorkFactory _unitOfWorkFactory;
        private List<LanguageDto> _languageDtos = new List<LanguageDto>();
        private List<WordDto> _wordDtos = new List<WordDto>();
        private List<WordTranslationDto> _wordTranslationDtos = new List<WordTranslationDto>();

        private const string _titleOfPreSetMultiAddToDateBaseOperation = "Анализ текста";
        private const string _titleOfAnalizeDateOperation = "Анализ данных";
        private const string _titleOfMultiAddToDateBaseOperation = "Сохранение данныъ";

        DictionaryOfWords.SignalR.ProgressBar _progressBar = null;
        public MultiAddToBaseService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
        private int _countMultiAddToDateBase = 0;
        public int CountMultiAddToDateBase { get { return _countMultiAddToDateBase; } }

        public List<WordTranslationDto> WordTranslations { get { return _wordTranslationDtos; } }

        public List<WordDto> Words { get { return _wordDtos; } }

        public void DoGenerate(IHubContext<DictionaryOfWords.SignalR.ProgressHub> progressHubContext, string text)
        {
            _progressBar = new DictionaryOfWords.SignalR.ProgressBar(progressHubContext);
            PreSetMultiAddToDateBase(text);
            AnalizeDate();
            MultiAddToDateBase();
        }

        /// <summary>
        /// Анализ данных (что нужно добавить и что нет)
        /// </summary>
        private void AnalizeDate()
        {
            if (_languageDtos.Count != 2) return;
            List<Language> languages = new List<Language>();
            List<Word> words = new List<Word>();
            List<WordTranslation> wordTranslations = new List<WordTranslation>();

            LanguageDto language;
            WordDto word;
            WordTranslationDto wordTranslation;

            int index = 0;

            if (_languageDtos == null || _languageDtos.Count == 0 || _languageDtos.Count != 2)
            {
                _progressBar.SendProgress(_countMultiAddToDateBase, _countMultiAddToDateBase, _titleOfAnalizeDateOperation);
                return;
            }

            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                List<string> names = _languageDtos.Select(x => x.Name.Trim()).ToList();
                languages = unitOfWork.Language.GetLanguageListOfName(names);
                names = _wordDtos.Select(x => x.Name.Trim()).ToList();

                if (languages.Count > 0)
                {
                    int idFromLanguage = languages[0].Id;
                    int idToLanguage = languages.Count > 1 ? languages[1].Id : -1;
                    words = unitOfWork.Word.GetWordsForTwoLanguage(names, idFromLanguage, idToLanguage);

                    List<int> wordIdList = words.Select(x => x.Id).ToList();
                    if (wordIdList?.Count > 0)
                    {
                        wordTranslations = unitOfWork.WordTranslation.GetLanguageListOfName(wordIdList, idFromLanguage, idToLanguage);
                    }
                }
            }

            int iteration = 0;
            _countMultiAddToDateBase = languages.Count + _wordDtos.Count + words.Count + _wordTranslationDtos.Count + wordTranslations.Count;
            for (int i = 0; i < languages.Count; i++)
            {
                language = _languageDtos.FirstOrDefault(x => x.Name.Trim().ToLower() == languages[i].Name.Trim().ToLower());
                if (language != null)
                {
                    _languageDtos[_languageDtos.IndexOf(language)].IsAdd = false;
                    _languageDtos[_languageDtos.IndexOf(language)].Id = languages[i].Id;
                }
                iteration++;
                _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfAnalizeDateOperation);
            }

            for (int i = 0; i < _wordDtos.Count; i++)
            {
                _wordDtos[i].LanguageId = (_wordDtos[i].Language != null) ? _wordDtos[i].Language.Id : -1;
                iteration++;
                _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfAnalizeDateOperation);
            }

            for (int i = 0; i < words.Count; i++)
            {
                word = _wordDtos.FirstOrDefault(x => x.Name.Trim().ToLower() == words[i].Name.Trim().ToLower() && x.Language?.Id == words[i].LanguageId);
                if (word != null)
                {
                    _wordDtos[_wordDtos.IndexOf(word)].IsAdd = false;
                    _wordDtos[_wordDtos.IndexOf(word)].Id = words[i].Id;
                }
                iteration++;
                _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfAnalizeDateOperation);
            }

            for (int i = 0; i < _wordTranslationDtos.Count; i++)
            {
                _wordTranslationDtos[i].LanguageFromId = (_wordTranslationDtos[i].LanguageFromWord != null) ? _wordTranslationDtos[i].LanguageFromWord.Id : -1;
                _wordTranslationDtos[i].LanguageToId = (_wordTranslationDtos[i].LanguageToWord != null) ? _wordTranslationDtos[i].LanguageToWord.Id : -1;
                _wordTranslationDtos[i].WordSourceId = (_wordTranslationDtos[i].WordSource != null) ? _wordTranslationDtos[i].WordSource.Id : -1;
                _wordTranslationDtos[i].WordTranslationId = (_wordTranslationDtos[i].WordTranslationValue != null) ? _wordTranslationDtos[i].WordTranslationValue.Id : -1;
                iteration++;
                _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfAnalizeDateOperation);
            }

            for (int i = 0; i < wordTranslations.Count; i++)
            {
                wordTranslation =  _wordTranslationDtos.FirstOrDefault(x => (x.WordSourceId == wordTranslations[i].WordSource?.Id &&
                                                                            x.WordTranslationId == wordTranslations[i].WordTranslationValue?.Id &&
                                                                            x.LanguageFromId == wordTranslations[i].LanguageFromWord?.Id &&
                                                                            x.LanguageToId == wordTranslations[i].LanguageToWord?.Id) ||
                                                                     (x.WordSourceId == wordTranslations[i].WordTranslationValue?.Id &&
                                                                      x.WordTranslationId == wordTranslations[i].WordSource?.Id &&
                                                                      x.LanguageToId == wordTranslations[i].LanguageFromWord?.Id &&
                                                                      x.LanguageFromId == wordTranslations[i].LanguageToWord?.Id));
                if (wordTranslation != null)
                {
                    _wordTranslationDtos[_wordTranslationDtos.IndexOf(wordTranslation)].IsAdd = false;
                    _wordTranslationDtos[_wordTranslationDtos.IndexOf(wordTranslation)].Id = wordTranslations[i].Id;
                }
                iteration++;
                _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfAnalizeDateOperation);
            }
        }
        /// <summary>
        /// Добавление в БД
        /// </summary>
        private async void MultiAddToDateBase()
        {
            if (_languageDtos.Count != 2) return;
            List<LanguageDto> languages = _languageDtos.Where(x => x.IsAdd).ToList();
            List<WordDto> words = _wordDtos.Where(x => x.IsAdd).ToList();
            List<WordTranslationDto> wordTranslations = _wordTranslationDtos.Where(x => x.IsAdd).ToList();

            Language language;
            Word word;
            WordTranslation wordTranslation;

            if (languages.Count == 0 && words.Count == 0 && wordTranslations.Count == 0)
            {
                _progressBar.SendProgress(_countMultiAddToDateBase, _countMultiAddToDateBase, _titleOfMultiAddToDateBaseOperation);
                return;
            }
            Dictionary<LanguageDto, Language> languageKeyList = new Dictionary<LanguageDto, Language>();
            Dictionary<WordDto, Word> wordKeyList = new Dictionary<WordDto, Word>();
            using (var unitOfWork = _unitOfWorkFactory.MakeUnitOfWork())
            {
                int iteration = 0;
                _countMultiAddToDateBase = languages.Count + words.Count + wordTranslations.Count;
                if (languages?.Count > 0)
                {
                    for (int i = 0; i < languages.Count; i++)
                    {
                        language = AutoMapper.Mapper.Map<Language>(languages[i]);
                        language = await unitOfWork.Language.AddAsync(language);

                        languageKeyList.Add(languages[i], language);

                        iteration++;
                        _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfMultiAddToDateBaseOperation);
                    }
                    await unitOfWork.CompleteAsync();

                    _countMultiAddToDateBase += languageKeyList.Count;
                    foreach (KeyValuePair<LanguageDto, Language> rec in languageKeyList)
                    {
                        if (languages[languages.IndexOf(rec.Key)] != null)
                        {
                            languages[languages.IndexOf(rec.Key)].Id = rec.Value.Id;
                        }

                        iteration++;
                        _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfMultiAddToDateBaseOperation);
                    }
                }

                if (words?.Count > 0)
                {
                    for (int i = 0; i < words.Count; i++)
                    {
                        words[i].LanguageId = (words[i].Language != null) ? words[i].Language.Id : -1;
                        word = AutoMapper.Mapper.Map<Word>(words[i]);
                        word = await unitOfWork.Word.AddAsync(word);
                        wordKeyList.Add(words[i], word);

                        iteration++;
                        _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfMultiAddToDateBaseOperation);
                    }

                    await unitOfWork.CompleteAsync();

                    _countMultiAddToDateBase += wordKeyList.Count;

                    foreach (KeyValuePair<WordDto, Word> rec in wordKeyList)
                    {
                        if (words[words.IndexOf(rec.Key)] != null)
                        {
                            words[words.IndexOf(rec.Key)].Id = rec.Value.Id;
                        }

                        iteration++;
                        _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfMultiAddToDateBaseOperation);
                    }
                }
                if (wordTranslations?.Count > 0)
                {
                    for (int i = 0; i < wordTranslations.Count; i++)
                    {
                        wordTranslations[i].LanguageFromId = (wordTranslations[i].LanguageFromWord != null) ? wordTranslations[i].LanguageFromWord.Id : -1;
                        wordTranslations[i].LanguageToId = (wordTranslations[i].LanguageToWord != null) ? wordTranslations[i].LanguageToWord.Id : -1;
                        wordTranslations[i].WordSourceId = (wordTranslations[i].WordSource != null) ? wordTranslations[i].WordSource.Id : -1;
                        wordTranslations[i].WordTranslationId = (wordTranslations[i].WordTranslationValue != null) ? wordTranslations[i].WordTranslationValue.Id : -1;
                        wordTranslation = AutoMapper.Mapper.Map<WordTranslation>(wordTranslations[i]);
                        wordTranslation = await unitOfWork.WordTranslation.AddAsync(wordTranslation);

                        iteration++;
                        _progressBar.SendProgress(iteration, _countMultiAddToDateBase, _titleOfMultiAddToDateBaseOperation);
                    }

                    await unitOfWork.CompleteAsync();
                }
            }
        }

        /// <summary>
        /// Анализ текста преобразование в объект
        /// </summary>
        /// <param name="text">Текст</param>
        private void PreSetMultiAddToDateBase(string text)
        {
            _languageDtos.Clear();
            _wordDtos.Clear();
            _wordTranslationDtos.Clear();

            string[] lineList = text.Split("\n");
            _countMultiAddToDateBase = lineList.Length;

            for (int i = 0; i < lineList.Length; i++)
            {
                if (i == 0)
                {
                    SetLanguage(lineList[i]);
                    if (_languageDtos.Count != 2) break;
                    _progressBar.SendProgress(i, _countMultiAddToDateBase, _titleOfPreSetMultiAddToDateBaseOperation);
                }
                else
                {
                    SetWord(lineList[i]);
                    _progressBar.SendProgress(i, _countMultiAddToDateBase, _titleOfPreSetMultiAddToDateBaseOperation);
                }
            }
        }

        private void SetLanguage(string language)
        {
            string[] spaceWords = new string[2] { "—", "-" };
            string[] lines = language.Split(spaceWords, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 0) _languageDtos.Add(new LanguageDto { Id = -1, Name = lines[0].Trim(), IsAdd = true });
            if (lines.Length > 1 && lines[0].Trim() != lines[1].Trim()) _languageDtos.Add(new LanguageDto { Id = -1, Name = lines[1].Trim(), IsAdd = true });
        }

        private void SetWord(string words)
        {
            string[] spaceWords = new string[2] { "—", "-" };
            string[] lineWords = words.Split(spaceWords, StringSplitOptions.RemoveEmptyEntries);
            if (lineWords.Length != 2) return;
            List<WordTranslationDto> wordTranslationDtos = new List<WordTranslationDto>();
            AnalysisOfLine(lineWords[0].Trim(), wordTranslationDtos, true);
            AnalysisOfLine(lineWords[1].Trim(), wordTranslationDtos, false);
            for (int i = 0; i < wordTranslationDtos.Count; i++)
            {
                if (!_wordTranslationDtos.Any(x => x.LanguageFromWord == wordTranslationDtos[i].LanguageFromWord && x.LanguageToWord == wordTranslationDtos[i].LanguageToWord &&
                                                  x.WordSource == wordTranslationDtos[i].WordSource && x.WordTranslationValue == wordTranslationDtos[i].WordTranslationValue))
                {
                    _wordTranslationDtos.Add(wordTranslationDtos[i]);
                }
            }
        }

        private void AnalysisOfLine(string line, List<WordTranslationDto> wordTranslationDtos, bool isFirstLanguage = true)
        {
            if (line.Contains(","))
            {
                string[] lineWords = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lineWords.Length; i++)
                {
                    AnalysisOfWord(lineWords[i], wordTranslationDtos, isFirstLanguage);
                }
            }
            else
            {
                AnalysisOfWord(line, wordTranslationDtos, isFirstLanguage);
            }
        }

        private void AnalysisOfWord(string word, List<WordTranslationDto> wordTranslationDtos, bool isFirstLanguage = true)
        {
            if (word.Contains("w: "))
            {
                SetWordAndSound(word, wordTranslationDtos, isFirstLanguage);
            }
            else
            {
                SetJustWord(word, wordTranslationDtos, isFirstLanguage);
            }
        }

        private WordDto WordInsertInListAdd(string name, string pronunciation, bool isFirstLanguage)
        {
            LanguageDto languageTemp = (isFirstLanguage) ? _languageDtos[0] : _languageDtos[1];
            WordDto word = _wordDtos.FirstOrDefault(x => x.Name == name.Trim() && x.Language == languageTemp);
            if (word == null)
            {
                word = new WordDto
                {
                    Id = -1,
                    Name = name.Trim(),
                    Pronunciation = pronunciation.Trim(),
                    Language = (isFirstLanguage) ? _languageDtos[0] : _languageDtos[1],
                    IsAdd = true
                };
                _wordDtos.Add(word);
            }
            return word;
        }

        private void SetWordAndSound(string line, List<WordTranslationDto> wordTranslationDtos, bool isFirstLanguage = true)
        {
            string[] chStr = new string[2] { "w: ", "s: " };
            string[] lines = line.Split(chStr, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                string wordName = string.IsNullOrWhiteSpace(lines[0]) ? lines[1] : lines[0];
                string sound = string.IsNullOrWhiteSpace(lines[0]) ? lines[2] : lines[1];
                WordDto word = WordInsertInListAdd(wordName, sound, isFirstLanguage);
                SetWordTranslation(_wordDtos[_wordDtos.IndexOf(word)], wordTranslationDtos, isFirstLanguage);
            }
        }

        private void SetJustWord(string word, List<WordTranslationDto> wordTranslationDtos, bool isFirstLanguage = true)
        {
            WordDto wordDto = WordInsertInListAdd(word, string.Empty, isFirstLanguage); 
            SetWordTranslation(_wordDtos[_wordDtos.IndexOf(wordDto)], wordTranslationDtos, isFirstLanguage);
        }

        private void SetWordTranslation(WordDto word, List<WordTranslationDto> wordTranslationDtos, bool isFirstLanguage = true)
        {
            if (isFirstLanguage)
            {
                wordTranslationDtos.Add(
                    new WordTranslationDto 
                    {
                        Id = -1, 
                        LanguageFromWord = word.Language, 
                        WordSource = word, 
                        IsAdd = true 
                    });
            }
            else
            {
                if (wordTranslationDtos.Any(x => x.WordTranslationValue == null))
                {
                    for (int i = 0; i < wordTranslationDtos.Count; i++)
                    {
                        if (wordTranslationDtos[i].WordTranslationValue == null)
                        {
                            wordTranslationDtos[i].WordTranslationValue = word;
                            wordTranslationDtos[i].LanguageToWord = word.Language;
                        }
                    }
                }
                else
                {
                    List<WordDto> wordTempList = new List<WordDto>();
                    wordTempList = wordTranslationDtos.Select(x => x.WordSource).Distinct().ToList();
                    for (int i = 0; i < wordTempList.Count; i++)
                    {
                        wordTranslationDtos.Add(
                            new WordTranslationDto
                            {
                                Id = -1,
                                LanguageFromWord = wordTempList[i].Language,
                                WordSource = wordTempList[i],
                                LanguageToWord = word.Language,
                                WordTranslationValue = word,
                                IsAdd = true
                            }) ;
                    }
                }
            }
        }

    }
}
