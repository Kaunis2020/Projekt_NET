var kund_array = null;
$(document).ready(function () {
    anropaServer();
});

/* Anropar servern och får JSON-fil som respons; */
function anropaServer() {
    /* Hämtar sorterad JSON från SERVERN; */
    $.get({
        url: "/Admin/CustomerOnLoad/",
        success: function (data) {
            if (data !== "null") {
                kund_array = data;
            }             
        }
    });
}

/* Visar data i tabellform; */
function visaValet(str) {
    if ($("#laddat").length)
        $("#laddat").remove();
    if ($("#tabell table").length)
        $("#tabell table").remove();

    var table = $('<table></table>');

    if (str === "#" || str === "alla") {
        var th1 = "<tr><th>Nr</th><th>Kundtyp</th><th>Företag</th><th>Kontaktperson</th><th>Adress</th></tr>";
        table.append(th1);
        for (i = 0; i < kund_array.length; i++) {
            var col1 = $("<td></td>").text(kund_array[i].nummer);
            var col2 = $("<td></td>").text(kund_array[i].kundTyp);
            var col3;

            if (kund_array[i].kundTyp === "Företag") {
                col3 = $("<td></td>").text(kund_array[i].firmaNamn);
            }
            else {
                col3 = $("<td></td>").text(" --------- ");
            }

            var col4 = $("<td></td>").text(kund_array[i].kontaktPerson);
            var adress = kund_array[i].gatuAdress + ",  " + kund_array[i].ort + ",  " + kund_array[i].land;
            var col5 = $("<td></td>").text(adress);
            var filnamn = "../images/" + kund_array[i].bild;
            var rad = $('<tr></tr>');
            rad.attr('id', 'klickbar');
            rad.click({ param1: filnamn, param2: kund_array[i].kontaktPerson }, VisaKundBild);
            rad.append(col1);
            rad.append(col2);
            rad.append(col3);
            rad.append(col4);
            rad.append(col5);
            table.append(rad);
        }
        $("#tabell").append(table);

    } else {
        if (kategoriFinns(str)) {
            var th1 = "<tr><th>Nr</th><th>Kundtyp</th><th>Företag</th><th>Kontaktperson</th><th>Adress</th></tr>";
            table.append(th1);
            for (i = 0; i < kund_array.length; i++) {
                if (kund_array[i].kundTyp === str) {
                    var col1 = $("<td></td>").text(kund_array[i].nummer);
                    var col2 = $("<td></td>").text(kund_array[i].kundTyp);
                    var col3;

                    if (kund_array[i].kundTyp === "Företag") {
                        col3 = $("<td></td>").text(kund_array[i].firmaNamn);
                    }
                    else {
                        col3 = $("<td></td>").text(" --------- ");
                    }

                    var col4 = $("<td></td>").text(kund_array[i].kontaktPerson);
                    var adress = kund_array[i].gatuAdress + ",  " + kund_array[i].ort + ",  " + kund_array[i].land;
                    var col5 = $("<td></td>").text(adress);
                    var filnamn = "../images/" + kund_array[i].bild;
                    var rad = $('<tr></tr>');
                    rad.attr('id', 'klickbar');
                    rad.click({ param1: filnamn, param2: kund_array[i].kontaktPerson }, VisaKundBild);
                    rad.append(col1);
                    rad.append(col2);
                    rad.append(col3);
                    rad.append(col4);
                    rad.append(col5);
                    table.append(rad);
                }
            }
            $("#tabell").append(table);
        }
    }
}

function kategoriFinns(vald) {
    for (i = 0; i < kund_array.length; i++) {
        if (kund_array[i].kundTyp === vald) {
            return true;
        }
    }
    return false;
}