using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DictionaryOfWords.Service.Services.Contracts;
using DictionaryOfWords.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryOfWords.Web.Controllers
{
    public abstract class ControllerBaseWithFilter : Controller
    {
        public ControllerBaseWithFilter()
        { }

        public ControllerBaseWithFilter(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _mapper = mapper;
        }
        protected const int firstPageForDropList = 1;
        protected const int sizqListOnPageForDropList = 5;
        protected const int sizeListOnPage = 20;
        protected readonly IMapper _mapper;

        protected string GetError(string[] errors)
        {
            if (errors == null) return string.Empty;
            string error = string.Empty;
            for (int i = 0; i < errors.Length; i++)
            {
                error += errors[i] + Environment.NewLine;
            }
            return error;
        }

        protected ViewListModel GetViewListModel(int dtoListCount, string error)
        {
            var model = new ViewListModel();
            int firstPage = 0;

            if (dtoListCount != 0)
            {
                double pageCount = dtoListCount / sizeListOnPage;
                int pageTotalCount = (int)pageCount;
                int pageRest = dtoListCount % sizeListOnPage;
                if (pageRest != firstPage)
                {
                    pageTotalCount++;
                }
                model.PageCount = pageTotalCount;
            }
            else
            {
                model.PageCount = firstPage;
            }
            model.PageSize = sizeListOnPage;
            model.RowCount = sizeListOnPage;
            model.Error = error;
            return model;
        }
    }
}