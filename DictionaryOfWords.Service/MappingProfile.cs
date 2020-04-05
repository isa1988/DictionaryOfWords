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
            WordTranslationMapping();
        }

        private void LanguageMapping()
        {
            CreateMap<LanguageDto, Language>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.Words, p => p.Ignore())
                .ForMember(x => x.WordFromTranslations, p => p.Ignore())
                .ForMember(x => x.WordToTranslations, p => p.Ignore())
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name.Trim()));

            CreateMap<Language, LanguageDto>()
                .ForMember(x => x.IsAdd, p => p.Ignore());
        }

        private void WordMapping()
        {
            CreateMap<WordDto, Word>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.Language, p => p.Ignore())
                .ForMember(x => x.WordSources, p => p.Ignore())
                .ForMember(x => x.WordTranslations, p => p.Ignore())
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name.Trim()))
                .ForMember(x => x.Pronunciation, p => p.MapFrom(c => c.Pronunciation.Trim()));


            CreateMap<Word, WordDto>()
                .ForMember(x => x.IsAdd, p => p.Ignore());
        }

        private void WordTranslationMapping()
        {
            CreateMap<WordTranslationDto, WordTranslation>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.LanguageFromWord, p => p.Ignore())
                .ForMember(x => x.LanguageToWord, p => p.Ignore())
                .ForMember(x => x.WordSource, p => p.Ignore())
                .ForMember(x => x.WordTranslationValue, p => p.Ignore());


            CreateMap<WordTranslation, WordTranslationDto>();
        }
    }
}
