var emplarray = null;
$(document).ready(function () {
    anropaServer();
});

/* Anropar servern och får JSON-fil som respons; */
function anropaServer() {
    /* Hämtar sorterad JSON från SERVERN; */
    $.get({
        url: "/Admin/EmplOnLoad/",
        success: function (data) {
            if (data !== "null") {
                emplarray = data;
            }
        }
    });
}

/* Visar data i tabellform; */
function visaValet(str) {
    if ($("#laddat").length)
        $("#laddat").remove();
    if ($("#empltabell table").length)
        $("#empltabell table").remove();

    var table = $('<table></table>');
    if (kategoriFinns(str)) {
        var th1 = "<tr><th>Nr</th><th>ID</th><th>Namn</th><th>Avdelning</th><th>Epost</th><th>Telefon</th></tr>";
        table.append(th1);
        for (i = 0; i < emplarray.length; i++) {
            if (emplarray[i].department === str) {
                var fullname = emplarray[i].first_name + "  " + emplarray[i].last_name;
                var col1 = $("<td></td>").text(emplarray[i].nummer);
                var col2 = $("<td></td>").text(emplarray[i].id);
                var col3 = $("<td></td>").text(fullname);
                var col4 = $("<td></td>").text(emplarray[i].department);
                var col5 = $("<td></td>").text(emplarray[i].email);
                col5.addClass("epost");
                var col6 = $("<td></td>").text(emplarray[i].phone);
                var filnamn = "../images/" + emplarray[i].bild;
                var rad = $('<tr></tr>');
                rad.attr('id', 'klickbar');
                rad.click({ param1: filnamn, param2: fullname }, VisaKundBild);
                rad.append(col1);
                rad.append(col2);
                rad.append(col3);
                rad.append(col4);
                rad.append(col5);
                rad.append(col6);
                table.append(rad);
            }
        }
        $("#empltabell").append(table);
        $("#laddat").append('<br/>');
        $("#laddat").append('<br/>');
    }
}

function kategoriFinns(vald) {
    for (i = 0; i < emplarray.length; i++) {
        if (emplarray[i].department === vald) {
            return true;
        }
    }
    return false;
}