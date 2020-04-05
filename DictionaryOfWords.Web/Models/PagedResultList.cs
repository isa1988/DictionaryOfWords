using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web.Models
{
    public abstract class PagedResultList : PageInfoModel
    {
        public PagedResultList()
        {
            RowCount = 20;
            CurrentPage = 1;
        }
        [JsonProperty(PropertyName = "currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty(PropertyName = "pageCount")]
        public int PageCount { get; set; }
        
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }
        
        [JsonProperty(PropertyName = "rowCount")]
        public int RowCount { get; set; }

    }
}
