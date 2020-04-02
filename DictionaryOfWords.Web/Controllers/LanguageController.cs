using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using DictionaryOfWords.Web.Models;
using DictionaryOfWords.Web.Models.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryOfWords.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LanguageController : BaseController
    {
        private readonly ILanguageService _service;

        public LanguageController(ILanguageService service)
        {
            _service = service;
        }

        // GET: Language
        private ViewListModel GetViewListModel(string error, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }
            var languageDtoList = _service.GetAll();
            var model = new ViewListModel();
            var languageModelList = new List<LanguageModel>();
            languageModelList.Add(new LanguageModel());
            model.LanguageModels = languageModelList;
            if (languageDtoList.Count != 0)
            {
                double pageCount = languageDtoList.Count / 20;
                int pageTotalCount = (int)pageCount;
                int pageRest = languageDtoList.Count % 20;
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
            model.LanguageFilter = new LanguageFilterModel { Name = name };
            return model;
        }

        public IActionResult Index()
        {
            var model = GetViewListModel(string.Empty, string.Empty);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ViewListModel request)
        {
            var model = GetViewListModel(string.Empty, request.LanguageFilter?.Name);
            return View(model);
        }

        public IActionResult IndexError(ViewListModel request)
        {
            var model = GetViewListModel(request.Error, request.LanguageFilter?.Name);
            return View("Index", model);
        }


        [HttpPost]
        public ActionResult GetLanguagesFiveLines([FromBody] PageInfoNumberModel request)
        {
            if (request == null || request.LanguageFilter == null || string.IsNullOrWhiteSpace(request.LanguageFilter.Name)) return Json(string.Empty);
            var languageDtos = _service.GetAllOfPageFilter(1, 5, request.LanguageFilter.Name);
            var languageModels = AutoMapper.Mapper.Map<List<LanguageModel>>(languageDtos);
            return Json(languageModels);
        }

        [HttpPost]
        public ActionResult GetLanguageModelOfPage([FromBody] PageInfoNumberModel request)
        {
            var languageDtos = request.LanguageFilter == null
                          ? _service.GetAllOfPage(request.CurrentPage, 20)
                          : _service.GetAllOfPageFilter(request.CurrentPage, 20, request.LanguageFilter.Name);
            var languageModels = AutoMapper.Mapper.Map<List<LanguageModel>>(languageDtos);
            return Json(languageModels);
        }

        // GET: Language/Create
        public ActionResult Create()
        {
            return View(new LanguageModel { Title ="Создание новой записи" });
        }

        // POST: Language/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LanguageModel request)
        {
            var languageDto = AutoMapper.Mapper.Map<LanguageDto>(request);

            var result = await _service.CreateItemAsync(languageDto);

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

        // GET: Language/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var languageDto = _service.GetByID(id.Value);

            var language = AutoMapper.Mapper.Map<LanguageModel>(languageDto);
            language.Title = "Редактирование";

            return View(language);
        }

        // POST: Language/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LanguageModel request)
        {
            var languageDto = AutoMapper.Mapper.Map<LanguageDto>(request);
            
            var result = await _service.EditItemAsync(languageDto);

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


        public async Task<IActionResult> DeleteMultiJson([FromBody] ViewListModel request)
        {
            List<LanguageModel> languageModels = request.LanguageModels.Where(x => x.IsDelete).ToList();
            if (languageModels?.Count > 0)
            {
                DeleteMultiModel deleteMultiModel = new DeleteMultiModel { LanguageModels = languageModels };
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
            List<int> idList = request.LanguageModels.Where(x => x.IsDelete).Select(x => x.Id).ToList();
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
    }
}