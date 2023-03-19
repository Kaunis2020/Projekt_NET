// Adminsida-funktioner;
var ordlista = []; // Array med ordlista;
$(document).ready(function () {
    get_wordlist();
    $("#result").hide();
    $("#sokruta").keydown(function (event) {
        if (event.which === 13 || event.keyCode === 13) {
            SearchEmployee();
        }
    });
})

function get_wordlist() {
    /* Hämtar sorterad JSON från SERVERN; */
    $.get({
        url: "/Admin/Dictionary/",
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
        url: "/Admin/AutoEmployee/" + ord,
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
function SearchEmployee() {
    var ord = document.getElementById("sokruta").value;
    if (ord === null || ord === "")
        return;
    if (ord !== null && ord !== "")
        document.getElementById("sokruta").value = "";

    $.get({
        url: "/Admin/SearchAnyEmpl/" + ord,
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

function skapaTabell(data) {
    if ($("#tabell table").length) {
        $("#tabell table").remove();
    }
    var table = $('<table></table>');
    var th1 = "<tr><th>Nr</th><th>Namn</th><th>Avdelning</th><th>E-post</th><th>Telefon</th><th>Edit</th><th>Delete</th></tr>";
    table.append(th1);
    for (var x = 0; x < data.length; x++) {
        var namn = data[x].first_name + "  " + data[x].last_name;
        var col1 = $("<td></td>").text(data[x].nummer);
        var col2 = $("<td></td>").text(namn);
        var col3 = $("<td></td>").text(data[x].department);
        col1.addClass('klickamig');
        var filnamn = "../images/" + data[x].bild;
        var col4 = $("<td></td>").text(data[x].email);
        col4.attr('id', 'klickbar');
        col4.addClass('epost');
        var col5 = $("<td></td>").text(data[x].phone);
        var col6 = $("<td></td>").text("edit");
        col6.addClass('klickamig');
        col6.click({ param1: data[x].id }, VisaForm);
        var col7 = $("<td></td>").text("delete");
        col7.addClass('klickamig');
        col7.attr('id', 'varna');
        col7.click({ param1: data[x].id }, RaderaEmpl);
        var rad = $('<tr></tr>');
        col1.click({ param1: filnamn, param2: namn }, VisaBild);
        col4.click({ param1: filnamn, param2: namn }, VisaBild);
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

function VisaForm(event) {
    /* Hämtar sorterad JSON från SERVERN; */
    var empid = event.data.param1;
    $.get({
        url: "/Admin/SendEmplForm/" + empid,
        success: function (data) {
            if (data !== null && data !== "") {
                if ($("#formdiv").length) {
                    $("#formdiv").empty();
                }
                $("#formdiv").html(data);
            }
        }
    });
}

function RaderaEmpl(event) {
    var empid = event.data.param1;
    var yes = window.confirm("Vill Du verkligen RADERA anställd?");
    if (yes === false) {
        return;
    }
    else if (yes === true) {
        var empid = event.data.param1;
        $.ajax({
            url: "/Admin/DeleteEmpl/",
            method: "POST",
            data: {
                "emploid": empid
            },
            dataType: 'text',
            success: function (data) {
                if (data !== null && data !== "")
                    if (data === "OK") {
                        get_wordlist();
                        var meddel = data + " - Denna anställd har raderats!";
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

function VisaBild(event) {
    var modal = document.getElementById('myModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modalImg.src = event.data.param1;
    captionText.innerHTML = event.data.param2;
    modal.style.display = "block";
}

function closethis() {
    var modal = document.getElementById('myModal');
    modal.style.display = "none";
}

function close__this() {
    $('#formdiv').empty();
    var modal = document.getElementById('editruta');
    modal.style.display = "none";
}

function rensa() {
    $('#formdiv').empty();
}