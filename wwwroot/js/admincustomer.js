// För Kundhantering i ADMIN-sida;

function OnBegin() {
    $("#progress").show();
}

function OnFailure(response) {
    alert("Error occured.");
}

function OnSuccess(response) {
   $("#kund_form").html(response);
}

function OnComplete() {
    $("#progress").hide();
}