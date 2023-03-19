// Bonzi surfar
const SPRITE_WIDTH = 205;
const SPRITE_HEIGHT = 170;
const BORDER_WIDTH = 1;
const SPACING_WIDTH = 1;

var beginpoints = [
    { "x": 1000, "y": 320 },
    { "x": 1000, "y": 320 },
    { "x": 1200, "y": 2560 },
    { "x": 1200, "y": 2560 },
    { "x": 1400, "y": 2560 },
    { "x": 1400, "y": 2560 },
    { "x": 1600, "y": 2560 },
    { "x": 1600, "y": 2560 },
    { "x": 1800, "y": 2560 },
    { "x": 1800, "y": 2560 },
    { "x": 2000, "y": 2560 },
    { "x": 2200, "y": 2560 },
    { "x": 2400, "y": 2560 },
    { "x": 2600, "y": 2560 },
    { "x": 2800, "y": 2560 },
    { "x": 3000, "y": 2560 },
    { "x": 3200, "y": 2560 },
    { "x": 0, "y": 2720 },
    { "x": 200, "y": 2720 },
    { "x": 400, "y": 2720 },
    { "x": 600, "y": 2720 },
    { "x": 800, "y": 2720 },
    { "x": 1000, "y": 2720 },
    { "x": 1200, "y": 2720 },
    { "x": 1400, "y": 2720 },
    { "x": 1600, "y": 2720 },
    { "x": 1800, "y": 2720 },
    { "x": 2000, "y": 2720 },
    { "x": 2200, "y": 2720 },
    { "x": 2400, "y": 2720 },
    { "x": 2600, "y": 2720 },
    { "x": 2800, "y": 2720 },
    { "x": 400, "y": 0 }
];

var pointsleft = [
    { "x": 400, "y": 0 },
    { "x": 600, "y": 0 },
    { "x": 0, "y": 1920 },
    { "x": 800, "y": 0 },
    { "x": 1000, "y": 0 },
    { "x": 1200, "y": 0 },
    { "x": 1400, "y": 0 },
    { "x": 1600, "y": 0 },
    { "x": 1800, "y": 0 },
    { "x": 2800, "y": 1760 },
    { "x": 3000, "y": 1760 },
    { "x": 3200, "y": 1760 },
    { "x": 2800, "y": 1760 },
    { "x": 3000, "y": 1760 },
    { "x": 3200, "y": 1760 },
    { "x": 2800, "y": 1760 },
    { "x": 3000, "y": 1760 },
    { "x": 3200, "y": 1760 },
    { "x": 2800, "y": 1760 },
    { "x": 3000, "y": 1760 }
];

var pointsright = [
    { "x": 2800, "y": 2400 },
    { "x": 2600, "y": 2400 },
    { "x": 2600, "y": 2400 },
    { "x": 2400, "y": 2400 },
    { "x": 2400, "y": 2400 },
    { "x": 1400, "y": 1760 },
    { "x": 1400, "y": 1760 },
    { "x": 0, "y": 1920 },
    { "x": 0, "y": 1920 },
    { "x": 2200, "y": 2400 },
    { "x": 2000, "y": 2400 },
    { "x": 1800, "y": 2400 },
    { "x": 2000, "y": 2400 },
    { "x": 1800, "y": 2400 },
    { "x": 1600, "y": 2400 },
    { "x": 2600, "y": 1760 },
    { "x": 2400, "y": 1760 },
    { "x": 2200, "y": 1760 },
    { "x": 2000, "y": 1760 },
    { "x": 2200, "y": 1760 },
    { "x": 2000, "y": 1760 },
    { "x": 1800, "y": 1760 },
    { "x": 1600, "y": 1760 },
    { "x": 1800, "y": 1760 },
    { "x": 1600, "y": 1760 },
    { "x": 1600, "y": 1760 }
];
var canvas = document.createElement('canvas');
var element = document.getElementsByClassName("jumbotron")[0];
element.appendChild(canvas);
var actualwidth = $(".jumbotron").parent().width();
canvas.width = actualwidth - 60;
canvas.height = SPRITE_HEIGHT;
var context = canvas.getContext('2d');
context.imageSmoothingEnabled = false;
var spriteSheetURL = "./Agents/Bonzi/map.png";
var image = new Image(205, 170);
image.src = spriteSheetURL;
var startindex = 0;
var frameIndex = 0;
var staypos = -20;
var posx = -20;
deltax = 10;
var currentpoints = pointsleft;
var startframe;
var frame;
var done = false;

var timer = function apear() {
    if (startindex < (beginpoints.length)) {
        startframe = beginpoints[startindex];
        context.clearRect(
            0,
            0,
            canvas.width,
            canvas.height
        );
        context.drawImage(
            image,
            startframe.x,
            startframe.y,
            SPRITE_WIDTH,
            SPRITE_HEIGHT,
            staypos,
            0,
            SPRITE_WIDTH,
            SPRITE_HEIGHT
        );
        startindex += 1;
    }
    else {
        clearInterval(timer);
        timer = null;
        if (!done) {
            setInterval(animate, 250);
            done = true;
        }
        else return;
    }
}

function animate() {
    // once we hit the end of the cycle,
    // start again
    if (frameIndex === currentpoints.length) {
        frameIndex = 0;
    }
    if (posx >= actualwidth) {
        frameIndex = 0;
        currentpoints = pointsright;
        deltax = -10;
        posx -= 140;
    }
    else if (posx <= -50) {
        frameIndex = 0;
        currentpoints = pointsleft;
        deltax = 10;
    }
    frame = currentpoints[frameIndex];
    context.clearRect(
        0,
        0,
        canvas.width,
        canvas.height
    );
    context.drawImage(
        image,
        frame.x,
        frame.y,
        SPRITE_WIDTH,
        SPRITE_HEIGHT,
        posx,
        0,
        SPRITE_WIDTH,
        SPRITE_HEIGHT
    );
    frameIndex += 1;
    posx += deltax;
}

image.onload = function () {
    setInterval(timer, 160);    
}

