function GetFilter() {
    return {
        wordFrom: $("#filterWordFrom").text(),
        languageFrom: $("#filterLanguageFrom").text(),
        wordTo: $("#filterWordTo").text(),
        languageTo: $("#filterLanguageTo").text()
    };
}
