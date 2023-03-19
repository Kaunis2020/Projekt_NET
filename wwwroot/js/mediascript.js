function OnBegin() {
    $("#progress").show();
}

function OnFailure(response) {
    alert("Error occured.");
}

function OnSuccess(response) {
    $("#media_form").html(response);
}

function OnComplete() {
    $("#progress").hide();
}

function rensa() {
    $('#media_form').empty();
}