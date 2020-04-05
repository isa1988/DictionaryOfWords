using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryOfWords.Web.AppStart
{
    public static class AutoMapperServiceExtension
    {
        public static void AddAutoMapperCustom(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DictionaryOfWords.Web.MappingProfile());
                mc.AddProfile(new DictionaryOfWords.Service.MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
