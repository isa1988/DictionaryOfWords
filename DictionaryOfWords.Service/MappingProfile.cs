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
            WordMapping();
        }

        private void LanguageMapping()
        {
            CreateMap<LanguageDto, Language>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.Words, p => p.Ignore())
                .ForMember(x => x.WordFromTranslations, p => p.Ignore())
                .ForMember(x => x.WordToTranslations, p => p.Ignore())
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));
            CreateMap<Language, LanguageDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));
        }

        private void WordMapping()
        {
            CreateMap<WordDto, Word>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.Language, p => p.Ignore())
                .ForMember(x => x.WordSources, p => p.Ignore())
                .ForMember(x => x.WordTranslations, p => p.Ignore())
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.Pronunciation, p => p.MapFrom(c => c.Pronunciation));


            CreateMap<Word, WordDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Language, p => p.MapFrom(c => c.Language))
                .ForMember(x => x.WordSources, p => p.MapFrom(c => c.WordSources))
                .ForMember(x => x.WordTranslations, p => p.MapFrom(c => c.WordTranslations))
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.Pronunciation, p => p.MapFrom(c => c.Pronunciation));
        }
    }
}
