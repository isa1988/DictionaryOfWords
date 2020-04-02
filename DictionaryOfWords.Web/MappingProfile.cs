using AutoMapper;
using DictionaryOfWords.Service.Dtos;
using DictionaryOfWords.Web.Models;
using DictionaryOfWords.Web.Models.Language;
using DictionaryOfWords.Web.Models.Word;
using DictionaryOfWords.Web.Models.WordTranslation;
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
            WordTranslationMapping();
            AddMultiMapping();
        }

        private void LanguageMapping()
        {
            CreateMap<LanguageDto, LanguageModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.IsDelete, p => p.Ignore())
                .ForMember(x => x.Title, p => p.Ignore())
                .ForMember(x => x.Error, p => p.Ignore());
            
            CreateMap<LanguageModel, LanguageDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.IsAdd, p => p.Ignore());
        }

        private void WordMapping()
        {
            CreateMap<WordDto, WordModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.LanguageName, p => p.MapFrom(c => c.Language != null ? c.Language.Name : string.Empty))
                .ForMember(x => x.IsDelete, p => p.Ignore())
                .ForMember(x => x.LanguageList, p => p.Ignore())
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));

            CreateMap<WordDto, WordDeleteModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.LanguageName, p => p.MapFrom(c => c.Language != null ? c.Language.Name : string.Empty))
                .ForMember(x => x.IsDelete, p => p.Ignore())
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name));


            CreateMap<WordModel, WordDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageId, p => p.MapFrom(c => c.LanguageId))
                .ForMember(x => x.Name, p => p.MapFrom(c => c.Name))
                .ForMember(x => x.IsAdd, p => p.Ignore());
        }
        

        private void WordTranslationMapping()
        {
            CreateMap<WordTranslationDto, WordTranslationModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageFromId, p => p.MapFrom(c => c.LanguageFromId))
                .ForMember(x => x.LanguageToId, p => p.MapFrom(c => c.LanguageToId))
                .ForMember(x => x.LanguageFromName, p => p.MapFrom(c => c.LanguageFromWord != null ? c.LanguageFromWord.Name : string.Empty))
                .ForMember(x => x.LanguageToName, p => p.MapFrom(c => c.LanguageToWord != null ? c.LanguageToWord.Name : string.Empty))
                .ForMember(x => x.WordFromId, p => p.MapFrom(c => c.WordSourceId))
                .ForMember(x => x.WordToId, p => p.MapFrom(c => c.WordTranslationId))
                .ForMember(x => x.WordFromName, p => p.MapFrom(c => c.WordSource != null ? c.WordSource.Name : string.Empty))
                .ForMember(x => x.WordToName, p => p.MapFrom(c => c.WordTranslationValue != null ? c.WordTranslationValue.Name : string.Empty))
                .ForMember(x => x.IsDelete, p => p.Ignore());

            CreateMap<WordTranslationModel, WordTranslationDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageFromId, p => p.MapFrom(c => c.LanguageFromId))
                .ForMember(x => x.LanguageToId, p => p.MapFrom(c => c.LanguageToId))
                .ForMember(x => x.WordSourceId, p => p.MapFrom(c => c.WordFromId))
                .ForMember(x => x.WordTranslationId, p => p.MapFrom(c => c.WordToId));

            CreateMap<WordTranslationAddOrEditModel, WordTranslationDto>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageFromId, p => p.MapFrom(c => c.LanguageFromId))
                .ForMember(x => x.LanguageToId, p => p.MapFrom(c => c.LanguageToId))
                .ForMember(x => x.WordSourceId, p => p.MapFrom(c => c.WordFromId))
                .ForMember(x => x.WordTranslationId, p => p.MapFrom(c => c.WordToId));
        }

        private void AddMultiMapping()
        {
            CreateMap<WordTranslationDto, WordMultiModel>()
                .ForMember(x => x.Id, p => p.MapFrom(c => c.Id))
                .ForMember(x => x.LanguageFromId, p => p.MapFrom(c => c.LanguageFromId))
                .ForMember(x => x.LanguageToId, p => p.MapFrom(c => c.LanguageToId))
                .ForMember(x => x.LanguageFrom, p => p.MapFrom(c => c.LanguageFromWord))
                .ForMember(x => x.LanguageTo, p => p.MapFrom(c => c.LanguageToWord))
                .ForMember(x => x.WordFromId, p => p.MapFrom(c => c.WordSourceId))
                .ForMember(x => x.WordToId, p => p.MapFrom(c => c.WordTranslationId))
                .ForMember(x => x.WordFrom, p => p.MapFrom(c => c.WordSource))
                .ForMember(x => x.WordTo, p => p.MapFrom(c => c.WordTranslationValue));
        }
    }
}
