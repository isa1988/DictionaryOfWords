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
    public class WordTranslationController : Controller
    {
        IHubContext<ProgressHub> _progressHubContext;
        IMultiAddToBaseService _service;

        public WordTranslationController(IHubContext<ProgressHub> progressHubContext, IMultiAddToBaseService service)
        {
            _progressHubContext = progressHubContext;
            _service = service;
        }

        public IActionResult AddALot()
        {
            return View(new AddMultiModel { Text = ""});
        }

        [HttpPost]
        public IActionResult AddALot([FromBody] AddMultiModel request)
        {
            ProgressBar functions = new ProgressBar(_progressHubContext);
            _service.SetText(request.Text);
            foreach (int index in _service.PreSetMultiAddToDateBase())
            {
                functions.SendProgress(index, _service.CountMultiAddToDateBase, "Анализ текста");
            }

            foreach (int index in _service.AnalizeDate())
            {
                functions.SendProgress(index, _service.CountMultiAddToDateBase, "Анализ добавления");
            }

            _service.MultiAddToDateBase();
            functions.SendProgress(_service.CountMultiAddToDateBase, _service.CountMultiAddToDateBase, "Добавление");
            request.WordMultiModelList = AutoMapper.Mapper.Map<List<WordMultiModel>>(_service.WordTranslations);
            request.WordModelList = AutoMapper.Mapper.Map<List<WordModel>>(_service.Words);
            return Ok(request);
        }
    }
}