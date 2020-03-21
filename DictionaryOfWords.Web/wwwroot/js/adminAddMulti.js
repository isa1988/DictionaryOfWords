function StartProcess() {
    let viewModel = {
        text: $("#textIn").val()
    };
    console.log('Submitting form...');
    $.ajax({
        type: 'POST',
        url: '/WordTranslation/AddALot',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(viewModel),
        success: function (result) {
            console.log('Data received: ');
            console.log(result);
        },
        error: function () {
            alert('Error occured');
        }
    });
}