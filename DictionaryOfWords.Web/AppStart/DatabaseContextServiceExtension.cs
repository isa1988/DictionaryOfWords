using DictionaryOfWords.DAL.Data;
using DictionaryOfWords.DAL.Data.Contracts;
using DictionaryOfWords.DAL.Unit;
using DictionaryOfWords.DAL.Unit.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryOfWords.Web.AppStart
{
    public static class DatabaseContextServiceExtension
    {
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DbContextDictionaryOfWords>(options => options.UseMySql(connection));
            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            var optionsBuilder = new DbContextOptionsBuilder<DbContextDictionaryOfWords>();
            optionsBuilder.UseMySql(connection);
            services.AddSingleton<IDbContextFactory>(
                sp => new DbContextFactory(optionsBuilder.Options));
        }
    }
}
