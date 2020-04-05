using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Dtos.FilterDto;
using DictionaryOfWords.Service.Services.Contracts;
using DictionaryOfWords.Web.Models;
using DictionaryOfWords.Web.Models.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryOfWords.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LanguageController : ControllerBaseWithFilter
    {
        public LanguageController(ILanguageService service, IMapper mapper) : base(mapper)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            _service = service;
        }

        private readonly ILanguageService _service;


        public IActionResult Index()
        {
            var model = GetViewListModel(new LanguageFilterModel());
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ViewListModel request)
        {
            var model = GetViewListModel(request.LanguageFilter);
            return View(model);
        }

        public IActionResult IndexError(ViewListModel request)
        {
            var model = GetViewListModel(request.LanguageFilter, request.Error);
            return View("Index", model);
        }

        private ViewListModel GetViewListModel(LanguageFilterModel filter, string error = "")
        {
            var filterDto = _mapper.Map<LanguageFilterDto>(filter);
            int dtoListCount = _service.GetCountOfAllFilter(filterDto);
            var model = GetViewListModel(dtoListCount, error);
            model.LanguageFilter = filter;
            return model;
        }

        [HttpPost]
        public ActionResult GetLanguagesFiveLines([FromBody] PageInfoNumberModel request)
        {
            if (request == null || request.LanguageFilter == null || string.IsNullOrWhiteSpace(request.LanguageFilter.Name)) return Json(string.Empty);
            var filter = _mapper.Map<LanguageFilterDto>(request.LanguageFilter);
            var languageDtos = _service.GetAllOfPageFilter(filter, firstPageForDropList, sizqListOnPageForDropList);
            var languageModels = _mapper.Map<List<LanguageModel>>(languageDtos);
            return Json(languageModels);
        }

        [HttpPost]
        public ActionResult GetLanguageModelOfPage([FromBody] PageInfoNumberModel request)
        {
            List<LanguageDto> languageDtos;
            if (request.LanguageFilter == null)
            {
                languageDtos = _service.GetAllOfPage(request.CurrentPage, sizeListOnPage);
            }
            else
            {
                var filter = _mapper.Map<LanguageFilterDto>(request.LanguageFilter);
                languageDtos = _service.GetAllOfPageFilter(filter, request.CurrentPage, sizeListOnPage);
            }
            var languageModels = _mapper.Map<List<LanguageModel>>(languageDtos);
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
            var languageDto = _mapper.Map<LanguageDto>(request);

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

            var language = _mapper.Map<LanguageModel>(languageDto);
            language.Title = "Редактирование";

            return View(language);
        }

        // POST: Language/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LanguageModel request)
        {
            var languageDto = _mapper.Map<LanguageDto>(request);
            
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