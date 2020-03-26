let currentPage = 0;

function setButtonFurther() {
    let pageCount = +$('#PageCount').val();
    if (pageCount === 0) {
        $("#further").hide();
    }
    else if (currentPage >= pageCount) {
        $("#further").hide();
    }
}

function GetCustomers(nameFilter, filter, urlTo) {
    currentPage++;
    let viewModel = {
        currentPage: currentPage
    };
    viewModel[nameFilter] = filter;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(viewModel),
        url: urlTo,
        success: function (result) {
            OnSuccess(result, currentPage);
        }
    });
    setButtonFurther();
}