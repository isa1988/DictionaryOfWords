function move(percent) {
    let elem = document.getElementById("greenBar");
    let width = +/\d+/.exec(percent);
    elem.style.width = width + '%';
    elem.innerHTML = percent + '%';
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