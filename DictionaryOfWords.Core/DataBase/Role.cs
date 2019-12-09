using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DictionaryOfWords.Core.DataBase
{
    public class Role : IdentityRole<int>
    {
        public Role(String name) : base(name)
        {
        }
    }
}
