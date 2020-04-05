using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DictionaryOfWords.SignalR;
using DictionaryOfWords.SignalR.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DictionaryOfWords.Web.Controllers
{
    public class TestSignelRController : Controller
    {
        public TestSignelRController(IHubContext<ProgressHub> progressHubContext,
                                     IHubContext<ChatHub> hubContext)
        {
            if (progressHubContext == null)
                throw new ArgumentNullException(nameof(progressHubContext));
            if (hubContext == null)
                throw new ArgumentNullException(nameof(hubContext));
            _progressHubContext = progressHubContext;
            _hubContext = hubContext;
        }

        IHubContext<ProgressHub> _progressHubContext;
        IHubContext<ChatHub> _hubContext;

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexSend()
        {
            return View();
        }

        public JsonResult LongRunningProcess()
        {
            //THIS COULD BE SOME LIST OF DATA
            int itemsCount = 100;
            ProgressBar functions = new ProgressBar(_progressHubContext);

            for (int i = 0; i <= itemsCount; i++)
            {
                //SIMULATING SOME TASK
                Thread.Sleep(500);
                _progressHubContext.Clients.All.SendAsync("ProgressBarValue", i.ToString());
                //functions.SendProgress(i, itemsCount);
                
            }

            return Json("");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string product)
        {
            await _hubContext.Clients.All.SendAsync("Notify", $"Добавлено: {product} - {DateTime.Now.ToShortTimeString()}");
            return RedirectToAction("IndexSend");
        }
    }
}