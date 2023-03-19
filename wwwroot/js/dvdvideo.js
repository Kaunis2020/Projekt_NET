$(document).ready(function () {
    $('input').keydown(function (event) {
        if (event.which === 13 || event.keyCode === 13) {
            send(Media.DVD);
        }
    });
    anropaServer(Media.DVD);
});
