function setFinalId(nameOfInput, final) {
    let textInInput = $('#'+nameOfInput).val();
    let searchId = $('#' + nameOfInput + 'List option').filter(function () {
        if (this.value === textInInput) {
            return this.dataset.value;
        }
    });
    let id = searchId.length > 0 ? +searchId[0].dataset.value : -1;
    let name = searchId.length > 0 ? searchId[0].value : "";
    $('#' + final + "Name").val(name);
    $('#' + final + "Id").val(id);
}

function OnSuccess(data, nameOfInputList) {
    $("#" + nameOfInputList).children('option').remove();
    for (let i = 0; i < data.length; i++) {
        $("#" + nameOfInputList).append($("<option>").attr('data-value', data[i].id).attr('value', data[i].name));
    }
}

function OnListClean(nameOfInputList) {
    $("#" + nameOfInputList).children('option').remove();
}

function wordCleanValue(nameOfInput, nameOfInputForm) {
    OnListClean(nameOfInput + 'List');
    $('#' + nameOfInput).val('');
    $('#' + nameOfInputForm + "Id").val(-1);
    $('#' + nameOfInputForm + "Name").val("");
}

function OnSelectToBase(dataFilter, nameOfInputList, urlTo) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(dataFilter),
        url: urlTo,
        success: function (result) {
            OnSuccess(result, nameOfInputList);
        },
        error: function (xhr) {
            OnListClean(nameOfInputList);
        }
    });
}

function genericLanguageSelect(nameOfInput, key, urlTo) {
    let word = $('#' + nameOfInput).val() + key;

    let dataFilter = { "languageFilter": { name: word } };
    OnSelectToBase(dataFilter, nameOfInput + "List", urlTo);  
}

function genericWorkSelect(workInput, languageInput, key, urlTo) {
    let language = $('#' + languageInput).val();
    let word = $('#' + workInput).val() + key;

    let dataFilter = { "wordFilter": { name: word, language: language } };
    OnSelectToBase(dataFilter, workInput + "List", urlTo);  
}

function setInputVal(nameOfInput, idVal, valName) {
    if (valName) {
        $("#" + nameOfInput + "List").append($("<option>").attr('data-value', idVal).attr('value', valName));
        $("#" + nameOfInput).val(valName);
    }
}