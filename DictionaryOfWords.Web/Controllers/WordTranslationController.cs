using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryOfWords.SignalR;
using DictionaryOfWords.Web.Models;
using Microsoft.AspNetCore.Mvc;
using DictionaryOfWords.Service.Services.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace DictionaryOfWords.Web.Controllers
{
    public class WordTranslationController : BaseController
    {
        IHubContext<ProgressHub> _progressHubContext;
        IMultiAddToBaseService _serviceMultiAdd;
        IWordTranslationService _service;

        public IActionResult Index()
        {
            var wordTranslationDtoList = _service.GetAll();
            var wordTranslationList = AutoMapper.Mapper.Map<List<WordTranslationModel>>(wordTranslationDtoList);
            DeleteListModel model = new DeleteListModel();
            model.WordTranslationModels = wordTranslationList;
            return View(model);
        }
        
        public IActionResult IndexError(DeleteListModel request)
        {
            var wordTranslationDtoList = _service.GetAll();
            var wordTranslationList = AutoMapper.Mapper.Map<List<WordTranslationModel>>(wordTranslationDtoList);
            DeleteListModel model = new DeleteListModel();
            model.WordTranslationModels = wordTranslationList;
            model.Error = request.Error;
            return View("Index", model);
        }

        public WordTranslationController(IHubContext<ProgressHub> progressHubContext, IMultiAddToBaseService serviceMultiAdd, IWordTranslationService service)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMulti(DeleteListModel request)
        {
            List<int> idList = request.WordTranslationModels.Where(x => x.IsDelete).Select(x => x.Id).ToList();
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
    }
}