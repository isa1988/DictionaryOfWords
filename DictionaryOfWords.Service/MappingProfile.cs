using AutoMapper;
using DictionaryOfWords.Core.DataBase;
using DictionaryOfWords.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryOfWords.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LanguageMapping();
        }

        private void LanguageMapping()
        {
            CreateMap<LanguageDto, Language>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name)); 
            CreateMap<Language, LanguageDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name)); 
        }
    }
}
