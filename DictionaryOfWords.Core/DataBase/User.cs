using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Core.DataBase
{
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName { get; set; }

        //public short LockoutEnabled { get; set; }
        //public short EmailConfirmed { get; set; }
    }
}
