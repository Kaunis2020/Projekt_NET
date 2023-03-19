$(document).ready(function () {
    get_ordlista();
    $('input').keydown(function (event) {
        if (event.which === 13 || event.keyCode === 13) {
            send(Media.ALLT);
        }
    });
    anropaServer(Media.ALLT);
});