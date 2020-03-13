using AutoMapper;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryOfWords.Web
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LanguageMapping();
        }

        private void LanguageMapping()
        {
            CreateMap<LanguageDto, LanguageModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.Title, p => p.Ignore())
                .ForMember(x => x.Error, p => p.Ignore());
            CreateMap<LanguageModel, LanguageDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));
        }
    }
}
