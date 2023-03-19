var ordlista = []; // Array med ordlista;

$(document).ready(function () {
    $("#prodform").hide();
    get_wordlist();
    $("#sokruta").keydown(function (event) {
        if (event.which === 13 || event.keyCode === 13) {
            sokValfritt(3);
        }
    });
});

function get_wordlist() {
    /* Hämtar sorterad JSON från SERVERN; */
    $.ajax({
        url: "/Admin/Glossary/",
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
        url: "/Admin/Autocomplete/",
        data: { "word": ord },
        success:
            function (svar) {
                if (svar !== null && svar !== "") {
                    $("#result").html("Resultat visas nedan:");
                    skapaTabell(svar);
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
function skapaTabell(data) {
    if ($("#tabell table").length) {
        $("#tabell table").remove();
    }
    var table = $('<table></table>');
    var th1 = "<tr><th>Nr</th><th>Typ</th><th>Titel</th><th>Författare</th><th>Kategori</th><th>Pris, kr</th><th>Edit</th><th>Delete</th></tr>";
    table.append(th1);
    for (var x = 0; x < data.length; x++) {
        var typ = data[x].typ;
        var media = data[x].titel;
        var col0 = $("<td></td>").text(data[x].number);
        var col1 = $("<td></td>").text(data[x].typ);  
        var col2 = $("<td></td>").text(data[x].titel);
        var col3 = $("<td></td>").text(data[x].skapare);
        var col4 = $("<td></td>").text(data[x].genre);
        var col5 = $("<td></td>").text(data[x].price);
        var col6 = $("<td></td>").text("edit");
        col6.addClass('klickamig');
        col6.click({ param1: data[x].iD_Nr, param2: typ, param3: media }, VisaProdForm);
        var col7 = $("<td></td>").text("delete");
        col7.addClass('klickamig');
        col7.attr('id', 'varna');
        col7.click({ param1: data[x].iD_Nr }, RaderaProdukt);
        var filnamn = "../images/" + data[x].picture;
        var rad = $('<tr></tr>');
        col0.addClass('klickamig');
        col0.click({
            param0: data[x].typ,
            param1: filnamn, param2: data[x].skapare,
            param3: data[x].description, param4: data[x].prodYear
        }, showJQGenerell);
        rad.append(col0);
        rad.append(col1);
        rad.append(col2);
        rad.append(col3);
        rad.append(col4);
        rad.append(col5);
        rad.append(col6);
        rad.append(col7);
        table.append(rad);
    }
    $("#tabell").append(table);
    if ($("#taggs").val().length) {
        $("#taggs").val('');
    }
}

/*  Söker på valfria ord; */
function sokValfritt(alter) {
    var ord = document.getElementById("sokruta").value;
    if (ord === null || ord === "")
        return;
    if (ord !== null && ord !== "")
        document.getElementById("sokruta").value = "";

    $.ajax({
        method: "POST",
        cache: false,
        url: "/Admin/SearchWord/",
        data: { "choice": alter, "word": ord },
        success:
            function (svar) {
                if (svar !== null && svar !== "") {
                    $("#result").html("Resultat visas nedan:");
                    skapaTabell(svar);
                }
                else {
                    var x = document.getElementById("nyaudio");
                    x.play();
                    alert("INGET RESULTAT!");
                }
            }
    });
}

function VisaProdForm(event) {
    if ($("#prodform").is(":visible")) {
        document.getElementById("medtyp").value = "";
        document.getElementById("media").value = "";
        document.getElementById("prodid").value = "";
        document.getElementById("nypris").value = "";
        $("#prodform").hide();
    }
    $("#prodid").val(event.data.param1);
    $("#medtyp").val(event.data.param2);
    $("#media").val(event.data.param3);
    $("#medtyp").prop('disabled', true);
    $("#media").prop('disabled', true);
    $("#prodform").show();
}

function RaderaProdukt(event) {
    var yes = window.confirm("Vill Du verkligen RADERA produkt?");
    if (yes === false) {
        return;
    }
    else if (yes === true) {
        var prodid = event.data.param1;
        $.post({
            url: "/Admin/DeleteProduct",
            data: {
                "prodid": prodid
            },
            success: function (data) {
                if (data !== null && data !== "")
                    if (data === "OK") {
                        get_wordlist();
                        var meddel = data + " - Produkten har raderats!";
                        if ($("#tabell table").length) {
                            $("#tabell table").remove();
                        }
                        $("#result").html(meddel);
                        alert(meddel);
                    }
                    else {
                        var x = document.getElementById("nyaudio");
                        x.play();
                        alert(data + " - Tekniskt fel vid raderingen");
                    }
            }
        });
    }
}

function NyProduktPris() {
    var prodid = document.getElementById('prodid').value;
    var pris = document.getElementById('nypris').value;
    if (prodid === null || prodid === "") {
        $("#prodform").hide();
        return;
    }
    if (isNaN(pris)) {
        alert("Priset skall vara ett heltal !!!");
        return;
    }
    else if (!isNaN(pris)) {
        pris = Math.abs(pris);
    }
    document.getElementById("medtyp").value = "";
    document.getElementById("media").value = "";
    document.getElementById("prodid").value = "";
    document.getElementById("nypris").value = "";
    $.post({
        url: "/Admin/UpdateProduct",
        data: {
            "prodid": prodid,
            "price": pris
        },
        success: function (data) {
            if (data !== null && data !== "")
                if (data === "OK") {
                    var meddel = data + " - Priset har uppdaterats!";
                    $("#prodform").hide();
                    alert(meddel);
                }
                else {
                    $("#prodform").hide();
                    var x = document.getElementById("nyaudio");
                    x.play();
                    alert(data + " - Tekniskt fel vid uppdateringen");
                }
        }
    });
}

function close__this() {
    document.getElementById("medtyp").value = "";
    document.getElementById("media").value = "";
    document.getElementById("prodid").value = "";
    document.getElementById("nypris").value = "";
    $("#prodform").hide();
}
