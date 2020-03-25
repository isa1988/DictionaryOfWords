function convertTableToJson() {
    var rows = [];
    $('table tbody tr').each(function (i, n) {
        rows.push({
            id: $('#viewModel_' + i + '__Id').val(),
            isDelete: $('#viewModel_' + i + '__IsDelete').get(0).checked
        });
    });
    return rows;
}

function OnDeleteList(deleteModel, urlDelete, urlAfterDelete) {
    
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(deleteModel),
        url: urlDelete,
        success: function (result) {
            $("#messageResultOperation").text("Записи успешно удалены");
            location.href = urlAfterDelete;
        },
        error: function (xhr) {
            if (xhr.status === 200) {
                $("#messageResultOperation").text("Записи успешно удалены");
                location.href = urlAfterDelete;
            }
            else {
                $("#messageResultOperation").text(xhr.responseJSON.error);
            }
        }
    });
}