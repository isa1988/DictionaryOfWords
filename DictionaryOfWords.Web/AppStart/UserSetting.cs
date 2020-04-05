using DictionaryOfWords.Core.DataBase;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryOfWords.Web.AppStart
{
    public static class UserSetting
    {
        public static void AddSettingUserAutorization(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                //options.Password.RequiredUniqueChars = 1;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<DictionaryOfWords.DAL.Data.DbContextDictionaryOfWords>();
        }
    }
}
