// Adminsida-funktioner;
var ordlista = []; // Array med ordlista;
$(document).ready(function () {
    $("#kundadrform").hide();
    $("#result").hide();
    get_wordlist();
    $("#sokruta").keydown(function (event) {
        if (event.which === 13 || event.keyCode === 13) {
            SearchAny();
        }
    });
})

function get_wordlist() {
    /* Hämtar sorterad JSON från SERVERN; */
    $.get({
        url: "/Admin/Vocabulary/",
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
            noResults: noResult("Inga resultat"),
            results: function (amount) {
                displayResult(amount);
            }
        },
        response: function (event, ui) {
            $("#result").show();
        },
        select: function (event, ui) {
            sendAuto(ui.item.label);
        }
    });
}

function noResult(message) {
    document.getElementById("result").innerHTML = message;
}

function displayResult(number) {
    document.getElementById("result").innerHTML = "Totalt " + number + " resultat är tillgängliga";
}

function sendAuto(label) {
    var ord = label;
    if (ord === null || ord === "")
        return;
    $.get({
        url: "/Admin/AutoComplete/" + ord,
        success:
            function (svar) {
                if (svar !== null && svar !== "") {
                    document.getElementById("result").innerHTML = "";
                    skapaTabell(svar);
                }
                else {
                    document.getElementById("result").innerHTML = "Inga resultat";
                    var x = document.getElementById("nyaudio");
                    x.play();
                    alert("INGET RESULTAT!");
                    
                }
            }
    });
}

// Funktionen skickar valda sökord till servern;
function SearchAny() {
    var ord = document.getElementById("sokruta").value;
    if (ord === null || ord === "")
        return;
    if (ord !== null && ord !== "")
        document.getElementById("sokruta").value = "";
    $.get({
        url: "/Admin/AnyCustomer/" + ord,
        success:
            function (svar) {
                if (svar !== null && svar !== "") {
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

/* Skapar tabell; Databasen ligger på servern och hämtas med
 * hjälp av en JSON-fil;  */
function skapaTabell(data) {
    if ($("#tabell table").length) {
        $("#tabell table").remove();
    }
    var table = $('<table></table>');
    var th1 = "<tr><th>Nr</th><th>Kundtyp</th><th>Företag</th><th>Kontaktperson</th><th>Adress</th><th>Edit</th><th>Delete</th></tr>";
    table.append(th1);
    for (var x = 0; x < data.length; x++) {
        var col1 = $("<td></td>").text(data[x].nummer);
        var col2 = $("<td></td>").text(data[x].kundTyp);
        var col3;
        col1.addClass('klickamig');
        if (data[x].kundTyp === "Företag") {
            col3 = $("<td></td>").text(data[x].firmaNamn);
        }
        else {
            col3 = $("<td></td>").text(" --------- ");
        }
        var filnamn = "../images/" + data[x].bild;
        var col4 = $("<td></td>").text(data[x].kontaktPerson);
        var adress = data[x].gatuAdress + ",  " + data[x].ort + ",  " + data[x].land;
        var col5 = $("<td></td>").text(adress);
        var col6 = $("<td></td>").text("edit");
        col6.addClass('klickamig');
        var kund;
        if (data[x].kundTyp === "Företag") {
            kund = data[x].firmaNamn;
        }
        else {
            kund = data[x].kontaktPerson;
        }
        col6.click({ param1: data[x].kund_ID, param2: kund }, VisaKundForm);
        var col7 = $("<td></td>").text("delete");
        col7.addClass('klickamig');
        col7.attr('id', 'varna');
        col7.click({ param1: data[x].kund_ID }, RaderaKund);
        var rad = $('<tr></tr>');
        col1.click({ param1: filnamn, param2: data[x].kontaktPerson }, VisaKundBild);
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

function closethis() {
    var modal = document.getElementById('myModal');
    modal.style.display = "none";
}

function close__this() {
    document.getElementById("kundid").value = "";
    document.getElementById("nygata").value = "";
    document.getElementById("nyort").value = "";
    $("#kundadrform").hide();
}

function VisaKundBild(event) {
    var modal = document.getElementById('myModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modalImg.src = event.data.param1;
    captionText.innerHTML = event.data.param2;
    modal.style.display = "block";
}

function VisaKundForm(event) {
    if ($("#kundadrform").is(":visible")) {
        document.getElementById("kundid").value = "";
        document.getElementById("kund").value = "";
        document.getElementById("nygata").value = "";
        document.getElementById("nyort").value = "";
        $("#kundadrform").hide();
    }
    $("#kundid").val(event.data.param1);
    $("#kund").val(event.data.param2);
    $("#kund").prop('disabled', true);
    $("#kundadrform").show();
}

function NyKundAdress() {
    var kundid = document.getElementById('kundid').value;
    var gata = document.getElementById('nygata').value;
    var ort = document.getElementById('nyort').value;
    var e = document.getElementById("landdrop");
    var land = e.options[e.selectedIndex].text;
    if (kundid === null || kundid === "") {
        deny();
    }
    if (gata === null || gata === "") {
        deny();
    }
    if (ort === null || ort === "") {
        deny();
    }
    if (land === null || land == "") {
        deny();
    }
    document.getElementById("kundid").value = "";
    document.getElementById("kund").value = "";
    document.getElementById("nygata").value = "";
    document.getElementById("nyort").value = "";
    $.post({
        url: "/Admin/CustNewAddr",
        data: {
            "kundid": kundid,
            "gata": gata,
            "ort": ort,
            "land": land
        },
        success: function (data) {
            if (data !== null && data !== "")
                if (data === "OK") {
                    var meddel = data + " - Adressen har uppdaterats!";
                    $("#kundadrform").hide();
                    alert(meddel);
                }
                else {
                    $("#kundadrform").hide();
                    var x = document.getElementById("nyaudio");
                    x.play();
                    alert(data + " - Tekniskt fel vid uppdateringen");
                }
        },
        error: function (data) {
            alert("Tekniskt fel vid uppdateringen!!!");
        }
    });
}

function RaderaKund(event) {
    var yes = window.confirm("Vill Du verkligen RADERA kunden?");
    if (yes === false) {
        return;
    }
    else if (yes === true) {
        var kund_id = event.data.param1;
        $.ajax({
            url: "/Admin/DeleteCustomer/",
            method: "POST",
            data: {
                "kundid": kund_id
            },
            dataType: 'text',
            success: function (data) {
                if (data !== null && data !== "")
                    if (data === "OK") {
                        get_wordlist();
                        var meddel = data + " - Kunden har raderats!";
                        if ($("#tabell table").length) {
                            $("#tabell table").remove();
                        }
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

function deny() {
    alert("Tomma fält får INTE förekomma!");
    var x = document.getElementById("nyaudio");
    x.play();
    $("#kundadrform").hide();
    return;
}