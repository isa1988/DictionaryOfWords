function move(percent) {
    let elem = document.getElementById("greenBar");
    let width = percent;
    elem.style.width = width + '%';
    elem.innerHTML = width + '%';
}
$(function () {
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/prog")
        .build();
    hubConnection.on('ProgressBarValue', function (message) {
        move(message);
    });
    hubConnection.start();
});