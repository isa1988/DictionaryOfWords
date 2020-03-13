using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryOfWords.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string GetError(List<string> errors)
        {
            string error = string.Empty;
            for (int i = 0; i < errors.Count; i++)
            {
                error += errors[i] + Environment.NewLine;
            }
            return error;
        }
    }
}