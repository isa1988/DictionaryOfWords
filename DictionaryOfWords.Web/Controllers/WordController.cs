using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using DictionaryOfWords.Service.Services.Contracts;
using DictionaryOfWords.Web.Models;
using DictionaryOfWords.Web.Models.Word;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DictionaryOfWords.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WordController : ControllerBaseWithFilter
    {
        public WordController(IWordService service, ILanguageService serviceLanguage, IMapper mapper) : base(mapper)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (serviceLanguage == null)
                throw new ArgumentNullException(nameof(serviceLanguage));
            _service = service;
            _serviceLanguage = serviceLanguage;
        }

        private readonly IWordService _service;
        private readonly ILanguageService _serviceLanguage;


        public IActionResult Index()
        {
            var model = GetViewListModel(new WordFilterModel());
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ViewListModel request)
        {
            var model = GetViewListModel(request.WordFilter);
            return View(model);
        }

        public IActionResult IndexError(ViewListModel request)
        {
            var model = GetViewListModel(request.WordFilter, request.Error);
            return View("Index", model);
        }

        private ViewListModel GetViewListModel(WordFilterModel filter, string error = "")
        {
            var filterDto = _mapper.Map<WordFilterDto>(filter);
            int dtoListCount = _service.GetCountOfAllFilter(filterDto);
            var model = GetViewListModel(dtoListCount, error);
            model.WordFilter = filter;
            return model;
        }

        [HttpPost]
        public ActionResult GetWordModelOfFiveLines([FromBody] PageInfoNumberModel request)
        {
            if (request == null || request.WordFilter == null || 
                string.IsNullOrWhiteSpace(request.WordFilter.Name) || string.IsNullOrWhiteSpace(request.WordFilter.LanguageName)) return Json(string.Empty);
            var filter = _mapper.Map<WordFilterDto>(request.WordFilter);
            var wordDtos = _service.GetAllOfPageFilter(filter, firstPageForDropList, sizeListOnPage);
            var wordModels = _mapper.Map<List<WordDeleteModel>>(wordDtos);
            return Json(wordModels);
        }


        [HttpPost]
        public ActionResult GetWordModelOfPage([FromBody] PageInfoNumberModel request)
        {
            List<WordDto> wordDtos;
            if (request.WordFilter == null)
            {
                wordDtos = _service.GetAllOfPage(request.CurrentPage, sizeListOnPage);
            }
            else
            {
                var filter = _mapper.Map<WordFilterDto>(request.WordFilter);
                wordDtos = _service.GetAllOfPageFilter(filter, request.CurrentPage, sizeListOnPage);
            }
            var wordModels = _mapper.Map<List<WordDeleteModel>>(wordDtos);
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
            var wordDto = _mapper.Map<WordDto>(request);

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

            var word = _mapper.Map<WordModel>(wordDto);
            word.LanguageList = GetLanguageList();
            word.Title = "Редактирование";

            return View(word);
        }

        // POST: Word/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WordModel request)
        {
            var wordDto = _mapper.Map<WordDto>(request);

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
            var languageList = _serviceLanguage.GetAll();
            return new SelectList(languageList, "Id", "Name");
        }
    }
}