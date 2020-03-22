using DictionaryOfWords.Service.Dtos;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using DictionaryOfWords.SignalR;

namespace DictionaryOfWords.Service.Services.Contracts
{
    public interface IMultiAddToBaseService
    {
        List<WordTranslationDto> WordTranslations { get; }
        List<WordDto> Words { get; }
        int CountMultiAddToDateBase { get; }
        
        void DoGenerate(IHubContext<ProgressHub> progressHubContext, string text);
    }
}
