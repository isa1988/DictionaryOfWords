using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.DAL
{
    static class HelperString
    {
        public static string Like(this string value)
        {
            return $"%{value}%";
        }
    }
}
