using DictionaryOfWords.SignalR.Contract;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.SignalR
{
    public class ChatHub : Hub<IChatHub>
    {
        public Task SendMessage(string messgage)
        {
            return Clients.All.SendMessage($"Добавлено: {messgage} - {DateTime.Now.ToShortTimeString()}");
        }
    }
}
