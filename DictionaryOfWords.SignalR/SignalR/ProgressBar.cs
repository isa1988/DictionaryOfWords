using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.SignalR.Contract;
using Microsoft.AspNetCore.SignalR;

namespace DictionaryOfWords.SignalR
{
    public class ProgressBar
    {
        public ProgressBar(IHubContext<ProgressHub> progressHubContext)
        {
            _progressHubContext = progressHubContext;
        }
        IHubContext<ProgressHub> _progressHubContext;

        public void SendProgress(int progressCount, int totalItems, string nameOperation)
        {
            var percentage = (progressCount * 100) / totalItems;
            _progressHubContext.Clients.All.SendAsync("ProgressBarValue", nameOperation + " "+ percentage.ToString() + "%");
        }
    }
}
