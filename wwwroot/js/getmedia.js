var bokArray = null; // Array med media;
var ordlista = []; // Array med ordlista;
// Mediatyper
const Media = {
    BOK: 0,
    CD: 1,
    DVD: 2,
    ALLT: 3
};

function get_ordlista() {
    /* Hämtar sorterad JSON från SERVERN; */
    $.ajax({
        url: "/Media/Glossary/",
        method: "POST",
        dataType: 'json',
        success: function (data) {
            if (data !== null && data !== "") {
                ordlista = data;
                show_choice(ordlista);
            }
        }
    });
}

// Visar hel ordlista som AUTOCOMPLETE;
function show_choice(reslist) {
    $("#taggs").focus().autocomplete({
        autoFocus: true,
        minLength: 2,
        source: reslist,
        messages: {
            noResults: function () {
                $("#result").html("Inga resultat");
            },
            results: function (amount) {
                displayResult(amount);
            }
        },
        select: function (event, ui) {
            sendAuto(ui.item.label);
        }
    });
}

function displayResult(amount) {
    $("#result").html("Totalt " + amount + " resultat är tillgängliga");
}

function sendAuto(label) {
    var ord = label;
    if (ord === null || ord === "")
        return;
    $.ajax({
        method: "POST",
        url: "/Media/Autocomplete/",
        data: { "word": ord },
        success:
            function (svar) {
                if (svar !== null && svar !== "") {
                    $("#result").html("Resultat visas nedan:");
                    skapaTabell(Media.ALLT, svar);
                }
                else {
                    $("#result").html("Inga resultat");
                    var x = document.getElementById("nyaudio");
                    x.play();
                    alert("INGET RESULTAT!");
                }
            }
    });
}

/* Anropar servern och får JSON-fil som respons; */
function anropaServer(alter) {
    /* Hämtar osorterad JSON från SERVERN; */
    $.get({
        url: "/Media/MediaOnLoad/" + alter,
        success: function (data) {
            if (data !== null && data !== "")
                bokArray = data;
        }
    });
}

function send(alter) {
    var ord = document.getElementById("sokruta").value;
    if (ord === null || ord === "")
        return;
    if (ord !== null && ord !== "")
        document.getElementById("sokruta").value = "";
    $.ajax({
        method: "POST",
        cache: false,
        url: "/Media/SearchWord/",
        data: { "choice": alter, "word": ord },
        success:
            function (svar) {
                if (svar !== null && svar !== "") {
                    $("#result").html("Resultat visas nedan:");
                    skapaTabell(alter, svar);
                }
                else {
                    $("#result").html("Inga resultat");
                    var x = document.getElementById("nyaudio");
                    x.play();
                    alert("INGET RESULTAT!");
                }
            }
    });
}

/* Skapar tabell; Databasen ligger på servern och hämtas med
 * hjälp av en JSON-fil;  */
function skapaTabell(number, data) {
    if ($("#tabell table").length) {
        $("#tabell table").remove();
    }
    var table = $('<table></table>');
    var th1 = "<tr><th>Nr</th><th>Typ</th><th>Titel</th><th>Författare</th><th>Kategori</th><th>Pris</th></tr>";
    table.append(th1);
    for (var x = 0; x < data.length; x++) {
        var col0 = $("<td></td>").text(data[x].number);
        var col1 = $("<td></td>").text(data[x].typ);      
        var col2 = $("<td></td>").text(data[x].titel);
        var col3 = $("<td></td>").text(data[x].skapare);
        var col4 = $("<td></td>").text(data[x].genre);
        var col5 = $("<td></td>").text(data[x].price);
        var filnamn = "../images/" + data[x].picture;
        var rad = $('<tr></tr>');
        rad.attr('id', 'klickbar');
        switch (number) {
            case 0: rad.click({ param1: filnamn, param2: data[x].skapare }, changeJQImage);
                break;
            case 1:
                var ljudfil = "../audio/" + data[x].sound;
                rad.click({ param1: filnamn, param2: data[x].skapare, param3: ljudfil }, changeJQAudio);
                break;
            case 2:
                rad.click({
                    param1: filnamn, param2: data[x].skapare,
                    param3: data[x].description, param4: data[x].prodYear
                }, changeJQStory);
                break;
            case 3:
                rad.click({
                    param0: data[x].genre, param1: filnamn,
                    param2: data[x].skapare,
                    param3: data[x].description, param4: data[x].prodYear
                }, showJQGenerell);
                break;
        }
        rad.append(col0);
        rad.append(col1);
        rad.append(col2);
        rad.append(col3);
        rad.append(col4);
        rad.append(col5);
        table.append(rad);
    }
    $("#tabell").append(table);
    if ($("#taggs").val().length) {
        $("#taggs").val('');
    }
}

/* Visar data i tabellform; */
function visaValet(number, str) {
    if (str === "#" || str === "alla") {
        skapaTabell(number, bokArray);
    } else {
        const selected = [];
        if (kategoriFinns(str)) {
            for (i = 0; i < bokArray.length; i++) {
                if (bokArray[i].genre === str) {
                    selected.push(bokArray[i]);
                }
            }
            if (selected.length > 0) {
                skapaTabell(number, selected);
            }
        }
        else {
            $("#tabell").html("<p id=\"result\">Inga resultat har hittats!</p>")
        }
    }
}

function kategoriFinns(vald) {
    for (i = 0; i < bokArray.length; i++) {
        if (bokArray[i].genre === vald) {
            return true;
        }
    }
    return false;
}