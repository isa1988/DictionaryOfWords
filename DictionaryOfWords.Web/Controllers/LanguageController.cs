using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Service.Services.Contracts;
using DictionaryOfWords.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryOfWords.Web.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly ILanguageService _service;

        public LanguageController(ILanguageService service)
        {
            _service = service;
        }

        // GET: Language
        public ActionResult Index()
        {
            var languageDtoList = _service.GetAll();
            var languageList = AutoMapper.Mapper.Map<List<LanguageModel>>(languageDtoList);
            return View(languageList);
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

        // GET: Language/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var languageDto = _service.GetByID(id.Value);

            var language = AutoMapper.Mapper.Map<LanguageModel>(languageDto);
            language.Title = "Удаление";

            return View(language);
        }

        // POST: Language/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(LanguageModel request)
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
    }
}