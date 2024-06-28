var canvas;

window.mountCanvas = () => {
    canvas = document.getElementById('mycanvas');
    ctx = canvas.getContext("2d");
    canvas.width = canvas.clientWidth;
    canvas.height = canvas.clientHeight;
    // 抗鋸齒
    // ref: https://www.zhihu.com/question/37698502
    if (window.devicePixelRatio) {
        canvas.style.width = canvas.width + "px";
        canvas.style.height = canvas.height + "px";
        canvas.height *=  window.devicePixelRatio;
        canvas.width *=  window.devicePixelRatio;
        ctx.scale(window.devicePixelRatio, window.devicePixelRatio);
    }
    canvas.addEventListener('mousedown', function (evt) {
        canvas.width = canvas.clientWidth;
        canvas.height = canvas.clientHeight;
        var mousePos = getMousePos(canvas, evt);
        ctx.beginPath();
        ctx.moveTo(mousePos.x, mousePos.y);
        evt.preventDefault();
        canvas.addEventListener('mousemove', mouseMove, false);
    });
    canvas.addEventListener('mouseup', function () {

        canvas.removeEventListener('mousemove', mouseMove, false);

    }, false);
    canvas.addEventListener('touchstart', function (evt) {
        // console.log('touchstart')
        // console.log(evt)
        var touchPos = getTouchPos(canvas, evt);
        ctx.beginPath();
        evt.preventDefault();
        canvas.addEventListener('touchmove', touchMove, false);
    });

    canvas.addEventListener('touchend', function () {
        // console.log("touchend")
        canvas.removeEventListener('touchmove', touchMove, false);
    }, false);


    // clear
    document.getElementById('clear').addEventListener('click', function () {
        // console.log("reset")
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    }, false);

}


function ConvertToImageClick (){
    var image = canvas.toDataURL("image/png");
    document.getElementById('image').innerHTML = "<img src='" + image + "' style='width: 400px; height: 200px;' />";

}
var dataURL
window.getImageBase64 = (i) => {
    try {
        if (!dataURL)
            dataURL = canvas.toDataURL("image/png")

        dataURLChunck = dataURL.substring(i * 10000, i * 10000 + 10000) ;
        console.log(dataURLChunck);
        return dataURLChunck;
    }
    catch (e) {
        cosole.log(e);
    }

}

function getMousePos(canvas, evt) {
    var rect = canvas.getBoundingClientRect();
    return {
        x: evt.clientX - rect.left,
        y: evt.clientY - rect.top
    };
}


function mouseMove(evt) {
    var mousePos = getMousePos(canvas, evt);
    ctx.lineCap = "round";
    ctx.lineWidth = 2;
    ctx.lineJoin = "round";
    ctx.shadowBlur = 1; // 邊緣模糊，防止直線邊緣出現鋸齒 
    ctx.shadowColor = 'black';// 邊緣顏色
    ctx.lineTo(mousePos.x, mousePos.y);
    ctx.stroke();
}
// touch
function getTouchPos(canvas, evt) {
    var rect = canvas.getBoundingClientRect();
    return {
        x: evt.touches[0].clientX - rect.left,
        y: evt.touches[0].clientY - rect.top
    };
}

function touchMove(evt) {
    // console.log("touchmove")
    var touchPos = getTouchPos(canvas, evt);
    // console.log(touchPos.x, touchPos.y)

    ctx.lineWidth = 2;
    ctx.lineCap = "round"; // 繪制圓形的結束線帽
    ctx.lineJoin = "round"; // 兩條線條交匯時，建立圓形邊角
    ctx.shadowBlur = 1; // 邊緣模糊，防止直線邊緣出現鋸齒 
    ctx.shadowColor = 'black'; // 邊緣顏色
    ctx.lineTo(touchPos.x, touchPos.y);
    ctx.stroke();
}


//window.alert = (msg) => alert(msg);

window.addOrientationChangeListener = () => {

    screen.orientation.addEventListener("change", (e) => {
        canvas.width = canvas.clientWidth;
        canvas.height = canvas.clientHeight;
        canvas.style.width = canvas.width  + "px";
        canvas.style.height = canvas.height+ "px";
        //if (screen.orientation.type == 'landscape-primary')
        //    document.getElementById("signature").className = document.getElementById("signature").className.replace(' content', ' content-landscape');
        //else
        //    document.getElementById("signature").className=  document.getElementById("signature").className.replace(' content-landscape', ' content');
    });
}

