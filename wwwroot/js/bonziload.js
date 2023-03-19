var myClippy = null;
const names = ["Bonzi"];
var datum;
var dag;
var manad;
var manad_namn = [];
var dag_namn = [];
var manadsnamn;
var dagsnamn;

$(function () {
    presentMe("Bonzi", "Hej på Dig, min objudne gäst!", "Företagets ägare är här!", "Trevligt att träffa Dig här!");
});

function presentMe(myname, repl1, repl2, repl3) {
    if (myClippy === null)
    {
        load_Clippy(myname, repl1, repl2, repl3);
    } else if (myClippy !== null) {
        myClippy.hide(false, load_Clippy.bind(this, myname, repl1, repl2, repl3));
    }
}

function load_Clippy(myname, repl1, repl2, repl3) {
    myClippy = null;
    if ($('#parent').length) {
        $('#parent').empty();
    }
    clippy.load(myname, function (agent) {
        myClippy = agent;
        countDate();
        var text = 'Hej, Hej! Här är jag,  ' + myname + '!!!!';
        myClippy.show();
        myClippy.play('Greet');
        myClippy.play('Idle1_1');
        myClippy.speak(text);
        myClippy.saySomething(dagsnamn, manadsnamn);
        myClippy.animate();
        myClippy.play('GetAttention');
        myClippy.play('Thinking');
        myClippy.speak(repl1);
        myClippy.play('Thinking');
        myClippy.play('Idle1_1');
        myClippy.speak(repl2);
        myClippy.gestureAt(400, 400);
        myClippy.play('Idle1_1');
        myClippy.speak(repl3);
        myClippy.play('Idle1_1');
        myClippy.speak('Jag är jätteglad att vara här!!!');
        if (names.indexOf(myname) !== -1) {
            myClippy.play('Pleased');
        }
        myClippy.play('Idle1_1');
        myClippy.animate();
        myClippy.play('Searching');
        myClippy.animate();
        myClippy.play('Idle1_1');
        myClippy.speak(repl1);
        myClippy.play('Thinking');
        myClippy.play('Idle1_1');
        myClippy.speak(repl2);
        myClippy.play('Idle1_1');
        myClippy.play('Thinking');
        myClippy.speak(repl3);
        myClippy.play('Idle1_1');
        myClippy.animate();
        myClippy.play('Congratulate');
        myClippy.play('Idle1_1');
        animateClippy(myClippy);
    });
}

function animateClippy(myClippy)
{
    var num = Math.floor(Math.random() * 2);
    var number = 0;
    if (num === 2)
        number = 11;
    else
        number = -11;
    var anim = myClippy.get_animations();
    var myreplics = myClippy.getSentence(number);
    var max = myreplics.length;
    var maxitem = max * 3;
    var items = mix_animation(anim, maxitem);
    var x = -1;
    for (var y = 0; y < max; y++)
    {
        myClippy.play('Idle1_1');
        myClippy.speak(myreplics[y]);
        myClippy.play(items[x += 1]);
        myClippy.play(items[x += 1]);
        myClippy.play(items[x += 1]);
        myClippy.play('Idle1_1');
    }
}

/* ALLA animationer, cirka 50; */
function play_all(myobje)
{
    var anim = myobje.get_animations();
    anim.forEach(elem => myobje.play(elem));
}

// Returnerar blandad array;
function mix_animation(array, maxvalue)
{
    var items = shuffle(array);
    var someitem = [];
    for (var i = 0; i < maxvalue; i++)
    {
        someitem.push(items[i]);
    }
    return someitem;
}

// Blandar arrayen;
function shuffle(array) {
    for (var i = array.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        var temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
    return array;
}

function countDate() {
    datum = new Date();
    dag = datum.getDay();
    manad = datum.getMonth();
    manad_namn = new Array("januari", "februari", "mars", "april", "maj", "juni", "juli", "augusti", "september", "oktober", "november", "december");
    manadsnamn = manad_namn[manad];
    dag_namn = new Array("söndag", "måndag", "tisdag", "onsdag", "torsdag", "fredag", "lördag");
    dagsnamn = dag_namn[dag];
}