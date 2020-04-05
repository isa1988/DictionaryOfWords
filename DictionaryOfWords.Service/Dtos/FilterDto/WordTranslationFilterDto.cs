namespace DictionaryOfWords.Service.Dtos.FilterDto
{
    public class WordTranslationFilterDto : FilterBaseDto
    {
        public string WordFrom { get; set; }

        public string LanguageFrom { get; set; }

        public string WordTo { get; set; }

        public string LanguageTo { get; set; }
    }
}
