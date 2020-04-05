using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryOfWords.Web.Models.WordTranslation;
using Microsoft.AspNetCore.Mvc;
using DictionaryOfWords.Service.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using DictionaryOfWords.Service.Dtos;
using System.Net.Http;
using DictionaryOfWords.Web.Models;
using Microsoft.AspNetCore.Authorization;
using DictionaryOfWords.Web.Models.Word;
using DictionaryOfWords.Service.Dtos.FilterDto;
using AutoMapper;

namespace DictionaryOfWords.Web.Controllers
{
    public class WordTranslationController : ControllerBaseWithFilter
    {
        public WordTranslationController(IHubContext<DictionaryOfWords.SignalR.ProgressHub> progressHubContext, IMultiAddToBaseService serviceMultiAdd,
                                        IWordTranslationService service, IMapper mapper) : base(mapper)
        {
            if (progressHubContext == null)
                throw new ArgumentNullException(nameof(progressHubContext));
            if (serviceMultiAdd == null)
                throw new ArgumentNullException(nameof(serviceMultiAdd));
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            _progressHubContext = progressHubContext;
            _serviceMultiAdd = serviceMultiAdd;
            _service = service;
        }

        private readonly IHubContext<DictionaryOfWords.SignalR.ProgressHub> _progressHubContext;
        private readonly IMultiAddToBaseService _serviceMultiAdd;
        private readonly IWordTranslationService _service;


        
        [Authorize(Roles = "User")]
        public IActionResult IndexUser()
        {
            var model = GetViewListModel(new WordTranslationFilterModel());
            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult IndexUser(ViewListModel request)
        {
            var model = GetViewListModel(request.WordTranslationFilter);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var model = GetViewListModel(new WordTranslationFilterModel());
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Index(ViewListModel request)
        {
            var model = GetViewListModel(request.WordTranslationFilter);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult IndexError(ViewListModel request)
        {
            var model = GetViewListModel(request.WordTranslationFilter, request.Error);
            return View("Index", model);
        }

        private ViewListModel GetViewListModel(WordTranslationFilterModel filter, string error = "")
        {
            var filterDto = _mapper.Map<WordTranslationFilterDto>(filter);
            int dtoListCount = _service.GetCountOfAllFilter(filterDto);
            var model = GetViewListModel(dtoListCount, error);
            model.WordTranslationFilter = filter;
            return model;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult GetDataOfPage([FromBody] PageInfoNumberModel request)
        {
            List<WordTranslationDto> wordTranslationDtos;
            if (request.WordTranslationFilter == null)
            {
                wordTranslationDtos = _service.GetAllOfPage(request.CurrentPage, sizeListOnPage);
            }
            else
            {
                var filter = _mapper.Map<WordTranslationFilterDto>(request.WordTranslationFilter);
                wordTranslationDtos = _service.GetAllOfPageFilter(filter, request.CurrentPage, sizeListOnPage);
            }
                          
            var wordTranslationModels = _mapper.Map<List<WordTranslationModel>>(wordTranslationDtos);
            return Json(wordTranslationModels);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMultiJson([FromBody] ViewListModel request)
        {
            List<WordTranslationModel> wordTranslationModels = request.WordTranslationModels.Where(x => x.IsDelete).ToList();
            if (wordTranslationModels?.Count > 0)
            {
                DeleteMultiModel deleteMultiModel = new DeleteMultiModel { WordTranslationModels = wordTranslationModels };
                return await DeleteMulti(deleteMultiModel);
            }
            else
            {
                return NotFound();
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(DeleteMultiModel request)
        {
            List<int> idList = request.WordTranslationModels.Select(x => x.Id).ToList();
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new WordTranslationAddOrEditModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (WordTranslationAddOrEditModel request)
        {
            var wordTranslation = _mapper.Map<WordTranslationDto>(request);
            var result = await _service.CreateItemAsync(wordTranslation);

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
        [Authorize(Roles = "Admin")]
        public IActionResult AddALot()
        {
            return View(new AddMultiModel { Text = "" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddALot([FromBody] AddMultiModel request)
        {
            _serviceMultiAdd.DoGenerate(_progressHubContext, request.Text);
            request.WordMultiModelList = _mapper.Map<List<WordMultiModel>>(_serviceMultiAdd.WordTranslations);
            request.WordModelList = _mapper.Map<List<WordModel>>(_serviceMultiAdd.Words);
            return Ok(request);
        }
    }
}