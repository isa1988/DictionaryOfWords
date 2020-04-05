using Autofac;
using DictionaryOfWords.DI;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryOfWords.Web.AppStart
{
    public static class AutofacServiceExtension
    {
        public static IContainer ConfigureAutofac(this IServiceCollection services)
        {
            var container = new AutofacContainer();
            return container.Build(services);
        }
    }
}
