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

        private ViewListModel GetViewListModel(string error, string name, string languageName)
        {
            var wordDtoList = _service.GetAllFilter(name, languageName);
            var model = new ViewListModel();
            var wordModelList = new List<WordDeleteModel>();
            wordModelList.Add(new WordDeleteModel());
            model.WordModels = wordModelList;
            if (wordDtoList.Count != 0)
            {
                double pageCount = wordDtoList.Count / 20;
                int pageTotalCount = (int)pageCount;
                int pageRest = wordDtoList.Count % 20;
                if (pageRest != 0)
                {
                    pageTotalCount++;
                }
                model.PageCount = pageTotalCount;
            }
            else
            {
                model.PageCount = 0;
            }
            model.PageSize = 20;
            model.RowCount = 20;
            model.Error = error;
            model.WordFilter = new WordFilterModel { LanguageName = languageName, Name = name };
            return model;
        }

        public IActionResult Index()
        {
            var model = GetViewListModel(string.Empty, string.Empty, string.Empty);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ViewListModel request)
        {
            var model = GetViewListModel(string.Empty, request.WordFilter?.Name, request.WordFilter?.LanguageName);
            return View(model);
        }

        public IActionResult IndexError(ViewListModel request)
        {
            var model = GetViewListModel(request.Error, request.WordFilter?.Name, request.WordFilter?.LanguageName);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult GetWordModelOfPage([FromBody] PageInfoNumberModel request)
        {
            var wordDtos = request.WordFilter == null 
                          ? _service.GetAllOfPage(request.CurrentPage, 20)
                          : _service.GetAllOfPageFilter(request.CurrentPage, 20, request.WordFilter.Name, request.WordFilter.LanguageName);
            var wordModels = AutoMapper.Mapper.Map<List<WordDeleteModel>>(wordDtos);
            return Json(wordModels);
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
        
        public async Task<IActionResult> DeleteMultiJson([FromBody] ViewListModel request)
        {
            List<WordDeleteModel> wordModels = request.WordModels.Where(x => x.IsDelete).ToList();
            if (wordModels?.Count > 0)
            {
                DeleteMultiModel deleteMultiModel = new DeleteMultiModel { WordModels = wordModels };
                return await DeleteMulti(deleteMultiModel);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(DeleteMultiModel request)
        {
            List<int> idList = request.WordModels.Where(x => x.IsDelete).Select(x => x.Id).ToList();
            var result = await _service.DeleteItemAsync(idList);

            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                request.Error = GetError(result.Errors);
                return NotFound(request);
            }
        }

        private SelectList GetLanguageList()
        {
            var languageList = _service.GetLanguageList();
            return new SelectList(languageList, "Id", "Name");
        }
    }
}