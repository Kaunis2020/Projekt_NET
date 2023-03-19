const slag = [{ "key": 5, "value": "Välj typ" }, { "key": 0, "value": "ALLT" }, { "key": 1, "value": "BOK" }, { "key": 2, "value": "CD" }, { "key": 3, "value": "DVD" }];

$(document).ready(function () {
    $("#formdiv").hide();
    $("#nydeparform").hide();
    $("#klick_a").click(function () {
        $("#formdiv").show();
        $("#nydeparform").show();
    });
    $("#klick_e").click(function () {
        displayByType();
    });
});

function addDepart() {
    var avdel = document.getElementById('nydepart').value;
    if (avdel === null || avdel === "") {
        $("#formdiv").hide();
        $("#nydeparform").hide();
        alert("Avdelning måste ha ett namn!")
        return;
    }
    else {
        document.getElementById('nydepart').value = "";
        $.ajax({
            url: "/Admin/NewDepartment/",
            method: "POST",
            data: {
                "newdepart": avdel
            },
            dataType: 'text',
            success: function (data) {
                if (data !== null && data !== "")
                    if (data === "OK") {
                        var meddel = data + " - Avdelning har lagts in!";
                        $("#formdiv").hide();
                        $("#nydeparform").hide();
                        alert(meddel);
                    }
                    else {
                        var x = document.getElementById("nyaudio");
                        x.play();
                        alert(data + " - Tekniskt fel för ny avdelning");
                    }
            }
        });
    }
}

function _closeThis() {
    document.getElementById("nydepart").value = "";
    $("#formdiv").hide();
}

function displayByType() {
    if ($("#output select").length) {
        $("#output select").remove();
        $("#output br").remove();
    }
    if ($("#output table").length) {
        $("#output table").remove();
    }
    var selcol = $("<select id='slag' name='slag'></select>");
    var opitem;
    slag.forEach((row) => {
        if (row.key === 5) {
            opitem = $("<option></option>").attr("value", row.key).prop('selected', true).text(row.value);
        } else {
            opitem = $("<option></option>").attr("value", row.key).text(row.value);
        }
        selcol.append(opitem);
    });
    selcol.change(getMedia);
    var modal = document.getElementById('output');
    $("#output").append(selcol).append("<br/><br/>");
    modal.style.display = "block";
}

function getMedia() {
    var valueSelected = this.value;

    if (valueSelected == 5) {
        displayInfo("Detta val är inte sökbart");
        return;
    } else {
        $.ajax({
            url: '../api/media/slag/' + valueSelected,
            type: 'GET',
            success: function (result) {
                reset();
                if (valueSelected == 0) {
                    createMediaTable(result);
                }
                else {
                    PrintTable(result);
                }
            },
            error: function (error) {
                reset();
                $('#notes').html("<b>" + "Ett fel har inträffat" + "</b>");
            }
        });
    }
}

function PrintTable(data) {
    if ($("#output table").length) {
        $("#output table").remove();
    }
    var table = $('<table></table>');
    for (var i = 0; i < data.length; i++) {
        var th0 = $("<th></th>").html("<b>" + data[i].type + ", varav:" + "</b>");
        var th1 = "<th>Antal</th>";
        var thrad = $("<tr></tr").append(th0);
        thrad.append(th1);
        table.append(thrad);
        var col1 = $("<td></td>").text("Totalt antal varutitlar");
        var col2 = $("<td></td>").text(data[i].number);
        var rad0 = $('<tr></tr>');
        rad0.append(col1);
        rad0.append(col2);
        table.append(rad0);
        for (var x = 0; x < data[i].genres.length; x++) {
            var col7 = $("<td></td>").text(data[i].genres[x].genre);
            var col8 = $("<td></td>").text(data[i].genres[x].number);
            var rad = $('<tr></tr>');
            rad.append(col7);
            rad.append(col8);
            table.append(rad);
        }
    }
    $("#output").append(table);
}

/* Tabell med mediabutikens produkter;  */
function createMediaTable(data) {
    if ($("#output table").length) {
        $("#output table").remove();
    }
    var table = $('<table></table>');
    var th1 = "<tr><th>Info om varulager</th><th>Antal</th></tr>";
    table.append(th1);
    var col1 = $("<td></td>").text("Totalt antal varutitlar");
    var col2 = $("<td></td>").text(data.length);
    var rad0 = $('<tr></tr>');
    rad0.append(col1);
    rad0.append(col2);
    table.append(rad0);

    for (var x = 0; x < data.genres.length; x++) {
        var col7 = $("<td></td>").text(data.genres[x].genre);
        var col8 = $("<td></td>").text(data.genres[x].number);
        var rad = $('<tr></tr>');
        rad.append(col7);
        rad.append(col8);
        table.append(rad);
    }
    var modal = document.getElementById('output');
    $("#output").append(table);
    modal.style.display = "block";
}

function reset() {
    $("#messages").find("h2").first().remove();
    $('#notes').html("");
}

function displayInfo(message) {
    var x = document.getElementById("nyaudio");
    x.play();
    alert(message);
}

/*  Stänger informationsrutan;  */
function close__this() {
    var modal = document.getElementById('output');
    modal.style.display = "none";
    if ($("#output select").length) {
        $("#output select").remove();
        $("#output br").remove();
    }
}

function clear_select() {
    if ($("#output select").length) {
        $("#output select").remove();
        $("#output br").remove();
    }
}

function deleteMessage(id_num) {
    var yes = window.confirm("Vill Du verkligen RADERA meddelandet?");
    if (yes === false) {
        return;
    } else if (yes === true) {
        var id = id_num;
        if (id === null || id === "")
            return;
        else {
            $.ajax({
                method: "GET",
                url: "/Admin/Delete/" + id,
                dataType: 'text',
                success:
                    function (svar) {
                        window.location.replace("../Admin/AllMessages");
                    },
                error: function (error) {
                    reset();
                    $('#notes').html("<b>" + error + "</b>");
                }
            });
        }
    }
}