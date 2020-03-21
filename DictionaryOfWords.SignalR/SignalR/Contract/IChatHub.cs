using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryOfWords.SignalR.Contract
{
    public interface IChatHub
    {
        Task SendMessage(string message);
    }
}
