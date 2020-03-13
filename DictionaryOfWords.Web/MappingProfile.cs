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
            WordMapping();
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

        private void WordMapping()
        {
            CreateMap<WordDto, WordModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.LanguageName, p => p.MapFrom(c => c.Language != null ? c.Language.Name : string.Empty))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));


            CreateMap<WordModel, WordDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));
        }
    }
}
