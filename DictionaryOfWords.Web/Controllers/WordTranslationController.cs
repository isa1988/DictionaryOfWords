using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryOfWords.Web.Models;
using Microsoft.AspNetCore.Mvc;
using DictionaryOfWords.Service.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using DictionaryOfWords.Service.Dtos;
using System.Net.Http;

namespace DictionaryOfWords.Web.Controllers
{
    public class WordTranslationController : BaseController
    {
        IHubContext<DictionaryOfWords.SignalR.ProgressHub> _progressHubContext;
        IMultiAddToBaseService _serviceMultiAdd;
        IWordTranslationService _service;

        private ViewListModel GetViewListModel(string error, string wordFrom, string languageFrom, string wordTo, string languageTo)
        {
            var wordTranslationDtoList = _service.GetAll();
            var model = new ViewListModel();
            var wordTranslationModelList = new List<WordTranslationModel>();
            wordTranslationModelList.Add(new WordTranslationModel());
            model.WordTranslationModels = wordTranslationModelList;
            if (wordTranslationDtoList.Count != 0)
            {
                double pageCount = wordTranslationDtoList.Count / 20;
                int pageTotalCount = (int)pageCount;
                int pageRest = wordTranslationDtoList.Count % 20;
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
            model.WordTranslationFilter = new WordTranslationFilterModel { WordFrom = wordFrom, LanguageFrom = languageFrom, WordTo = wordTo, LanguageTo = languageTo };
            return model;
        }


        public IActionResult Index()
        {
            var model = GetViewListModel(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ViewListModel request)
        {
            var model = GetViewListModel(string.Empty, request.WordTranslationFilter?.WordFrom, request.WordTranslationFilter?.LanguageFrom,
                                                       request.WordTranslationFilter?.WordTo, request.WordTranslationFilter?.LanguageTo);
            return View(model);
        }

        public IActionResult IndexError(ViewListModel request)
        {
            var model = GetViewListModel(request.Error, request.WordTranslationFilter?.WordFrom, request.WordTranslationFilter?.LanguageFrom,
                                                       request.WordTranslationFilter?.WordTo, request.WordTranslationFilter?.LanguageTo);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult GetDataOfPage([FromBody] PageInfoNumberModel request)
        {
            var wordTranslationDtos = request.WordTranslationFilter == null
                          ? _service.GetAllOfPage(request.CurrentPage, 20)
                          : _service.GetAllOfPageFilter(request.CurrentPage, 20, request.WordTranslationFilter?.WordFrom, request.WordTranslationFilter?.LanguageFrom,
                                                        request.WordTranslationFilter?.WordTo, request.WordTranslationFilter?.LanguageTo);
            var wordTranslationModels = AutoMapper.Mapper.Map<List<WordTranslationModel>>(wordTranslationDtos);
            return Json(wordTranslationModels);
        }


        [HttpPost]
        public ActionResult GetDataOfPage2(HttpRequestMessage request)
        {
            var result = request.Content.ReadAsStringAsync().Result;
            /*var wordTranslationDtos = request.WordTranslationFilter == null
                          ? _service.GetAllOfPage(request.CurrentPage, 20)
                          : _service.GetAllOfPageFilter(request.CurrentPage, 20, request.WordTranslationFilter?.WordFrom, request.WordTranslationFilter?.LanguageFrom,
                                                        request.WordTranslationFilter?.WordTo, request.WordTranslationFilter?.LanguageTo);
            var wordTranslationModels = AutoMapper.Mapper.Map<List<WordTranslationModel>>(wordTranslationDtos);
            return Json(wordTranslationModels);*/
            return View();
        }

        public WordTranslationController(IHubContext<DictionaryOfWords.SignalR.ProgressHub> progressHubContext, IMultiAddToBaseService serviceMultiAdd, IWordTranslationService service)
        {
            _progressHubContext = progressHubContext;
            _serviceMultiAdd = serviceMultiAdd;
            _service = service;
        }

        public IActionResult AddALot()
        {
            return View(new AddMultiModel { Text = ""});
        }

        [HttpPost]
        public IActionResult AddALot([FromBody] AddMultiModel request)
        {
            _serviceMultiAdd.DoGenerate(_progressHubContext, request.Text);
            request.WordMultiModelList = AutoMapper.Mapper.Map<List<WordMultiModel>>(_serviceMultiAdd.WordTranslations);
            request.WordModelList = AutoMapper.Mapper.Map<List<WordModel>>(_serviceMultiAdd.Words);
            return Ok(request);
        }

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
    }
}