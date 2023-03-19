/*  Visar valda objekt i ett minimerat fönster; */

// Denna funktion visar bilder med JQUERY;
function changeJQImage(event) {
    var modal = document.getElementById('myModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modalImg.src = event.data.param1;
    captionText.innerHTML = event.data.param2;
    modal.style.display = "block";
}

// Denna funktion visar bilder med klassisk JavaScript;
function changeImg(image, text) {
    var modal = document.getElementById('myModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modalImg.src = image;
    captionText.innerHTML = text;
    modal.style.display = "block";
}

function closethis() {
    var modal = document.getElementById('myModal');
    modal.style.display = "none";
}

// Denna funktion visar bilder med JQUERY;
function changeJQAudio(event) {
    var modal = document.getElementById('audioModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    var source = document.getElementById("musik");
    modalImg.src = event.data.param1;
    captionText.innerHTML = event.data.param2;
    source.src = event.data.param3;
    modal.style.display = "block";
}

function changeImgWithAudio(image, text, sound) {
    var modal = document.getElementById('audioModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    var source = document.getElementById("musik");
    modalImg.src = image;
    source.src = sound;
    captionText.innerHTML = text;
    modal.style.display = "block";
}

function closeaudio() {
    var modal = document.getElementById('audioModal');
    modal.style.display = "none";
}

// Denna funktion visar bilder med JQUERY;
function changeJQStory(event) {
    var modal = document.getElementById('videoModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("videocaption");
    var descrip = document.getElementById('vdstory');
    var produc = document.getElementById('year');
    descrip.innerHTML = event.data.param3;
    modalImg.src = event.data.param1;
    captionText.innerHTML = "Regissör: &nbsp;" + event.data.param2;
    produc.innerHTML = "Producerat: &nbsp;" + event.data.param4;
    modal.style.display = "block";
}

function changeImageStory(image, text, story, year) {
    var modal = document.getElementById('videoModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("videocaption");
    var descrip = document.getElementById('vdstory');
    var produc = document.getElementById('year');
    descrip.innerHTML = story;
    modalImg.src = image;
    captionText.innerHTML = "Regissör: &nbsp;" + text;
    produc.innerHTML = "Producerat: &nbsp;" + year;
    modal.style.display = "block";
}

function closevideo() {
    var modal = document.getElementById('videoModal');
    modal.style.display = "none";
}

function VisaKundBild(event) {
    var modal = document.getElementById('myModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modalImg.src = event.data.param1;
    captionText.innerHTML = event.data.param2;
    modal.style.display = "block";
}

// Denna funktion visar bilder med JQUERY;
function showJQGenerell(event) {
    var modal = document.getElementById('genModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("videocaption");
    var huvtyp = document.getElementById('typ');
    var descrip = document.getElementById('story');
    var produc = document.getElementById('year');
    huvtyp.innerHTML = "Typ:  &nbsp;" + event.data.param0;
    descrip.innerHTML = event.data.param3;
    modalImg.src = event.data.param1;
    captionText.innerHTML = "Skapad av:  &nbsp;" + event.data.param2;
    produc.innerHTML = "Producerat: &nbsp;" + event.data.param4;
    modal.style.display = "block";
}

// Denna funktion visar bilder;
function showOrdinary(typ, image, text, story, year, moms) {
    var modal = document.getElementById('genModal');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("videocaption");
    var huvtyp = document.getElementById('typ');
    var descrip = document.getElementById('story');
    var produc = document.getElementById('year');
    var momsdisp = document.getElementById('moms');
    huvtyp.innerHTML = "Typ:  &nbsp;" + typ;
    modalImg.src = image;
    captionText.innerHTML = "Skapad av:  &nbsp;" + text;
    descrip.innerHTML = story;
    produc.innerHTML = "Producerat: &nbsp;" + year;
    momsdisp.innerHTML = "Moms, SEK: &nbsp; " + moms;
    modal.style.display = "block";
}

function closeall() {
    var modal = document.getElementById('genModal');
    modal.style.display = "none";
}