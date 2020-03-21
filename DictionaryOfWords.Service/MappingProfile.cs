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
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));

            CreateMap<Language, LanguageDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.IsAdd, p => p.Ignore());
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
                .ForMember(x => x.Pronunciation, p => p.MapFrom(c => c.Pronunciation))
                .ForMember(x => x.IsAdd, p => p.Ignore());
        }

        private void WordTranslationMapping()
        {
            CreateMap<WordTranslationDto, WordTranslation>()
                .ForMember(x => x.Id, p => p.Ignore())
                .ForMember(x => x.LanguageFromId, p => p.MapFrom(c => c.LanguageFromId))
                .ForMember(x => x.LanguageToId, p => p.MapFrom(c => c.LanguageToId))
                .ForMember(x => x.WordSourceId, p => p.MapFrom(c => c.WordSourceId))
                .ForMember(x => x.WordTranslationId, p => p.MapFrom(c => c.WordTranslationId))
                .ForMember(x => x.LanguageFromWord, p => p.Ignore())
                .ForMember(x => x.LanguageToWord, p => p.Ignore())
                .ForMember(x => x.WordSource, p => p.Ignore())
                .ForMember(x => x.WordTranslationValue, p => p.Ignore());


            CreateMap<WordTranslation, WordTranslationDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageFromId, p => p.MapFrom(c => c.LanguageFromId))
                .ForMember(x => x.LanguageToId, p => p.MapFrom(c => c.LanguageToId))
                .ForMember(x => x.WordSourceId, p => p.MapFrom(c => c.WordSourceId))
                .ForMember(x => x.WordTranslationId, p => p.MapFrom(c => c.WordTranslationId))
                .ForMember(x => x.LanguageFromWord, p => p.MapFrom(c => c.LanguageFromWord))
                .ForMember(x => x.LanguageToWord, p => p.MapFrom(c => c.LanguageToWord))
                .ForMember(x => x.WordSource, p => p.MapFrom(c => c.WordSource))
                .ForMember(x => x.WordTranslationValue, p => p.MapFrom(c => c.WordTranslationValue));
        }
    }
}
