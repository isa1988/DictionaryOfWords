using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using DictionaryOfWords.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DictionaryOfWords.Web.Controllers
{
    public class WordController : BaseController
    {
        private readonly IWordService _service;

        public WordController(IWordService service)
        {
            _service = service;
        }

        // GET: Word
        public IActionResult Index()
        {
            var wordDtoList = _service.GetAll();
            var wordList = AutoMapper.Mapper.Map<List<WordDeleteModel>>(wordDtoList);
            ViewListModel model = new ViewListModel();
            model.WordModels = wordList;
            return View(model);
        }

        public IActionResult IndexError(ViewListModel request)
        {
            var wordDtoList = _service.GetAll();
            var wordList = AutoMapper.Mapper.Map<List<WordDeleteModel>>(wordDtoList);
            ViewListModel model = new ViewListModel();
            model.WordModels = wordList;
            model.Error = request.Error;
            return View("Index", model);
        }

        // GET: Word/Create
        public ActionResult Create()
        {
            return View(new WordModel
            {
                Title = "Создание новой записи",
                LanguageList = GetLanguageList(),
            });
        }

        // POST: Word/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WordModel request)
        {
            var wordDto = AutoMapper.Mapper.Map<WordDto>(request);

            var result = await _service.CreateItemAsync(wordDto);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //request.LanguageList = GetLanguageList();
                request.Error = GetError(result.Errors);
                return View(request);
            }
        }

        // GET: Word/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var wordDto = _service.GetByID(id.Value);

            var word = AutoMapper.Mapper.Map<WordModel>(wordDto);
            word.LanguageList = GetLanguageList();
            word.Title = "Редактирование";

            return View(word);
        }

        // POST: Word/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WordModel request)
        {
            var wordDto = AutoMapper.Mapper.Map<WordDto>(request);

            var result = await _service.EditItemAsync(wordDto);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //request.LanguageList = GetLanguageList();
                request.Error = GetError(result.Errors);
                return View(request);
            }
        }

        // GET: Word/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var wordDto = _service.GetByID(id.Value);

            var word = AutoMapper.Mapper.Map<WordModel>(wordDto);
            word.Title = "Удаление";

            return View(word);
        }

        // POST: Word/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(WordModel request)
        {
            var result = await _service.DeleteItemAsync(request.Id);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                request.Error = GetError(result.Errors);
                return View(request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(ViewListModel request)
        {
            List<int> idList = request.WordModels.Where(x => x.IsDelete).Select(x => x.Id).ToList();
            var result = await _service.DeleteItemAsync(idList);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                request.Error = GetError(result.Errors);
                return IndexError(request);
            }
        }

        private SelectList GetLanguageList()
        {
            var languageList = _service.GetLanguageList();
            return new SelectList(languageList, "Id", "Name");
        }
    }
}