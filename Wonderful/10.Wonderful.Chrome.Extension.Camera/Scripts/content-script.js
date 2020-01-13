//02-window捕捉到showwebcam消息，发送消息给chrome
window.addEventListener("message", function (e) {
    if (e.data.message == 'showwebcam') {
        var language = e.data.language;
        if (!language) {
            language = getCookie('CULTURE');
        }
        chrome.runtime.sendMessage(
            {
                message: 'showwebcam',
                taskid: e.data.taskid,
                runmode: e.data.runmode,
                filename: e.data.filename,
                cameratype: e.data.cameratype,
                machinename: e.data.machinename,
                capturecount: e.data.capturecount,
                capturefrequency: e.data.capturefrequency,
                imagevalues: e.data.imagevalues,
                language: language,
                saveurl: e.data.saveurl
            }, function (response) {
                //08-捕捉到后台发送消息并打印
                console.log('收到来自后台的回复：' + response);
            }
        );
    }
}, false);
//09-捕捉到webcamtakephotook消息，发送window消息
chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
    if (request.message == 'webcamtakephotook') {
        window.postMessage({ "message": "webcamtakephotook", "result": "" + request.result + "", "taskid": "" + request.taskid + "" }, '*');

        sendResponse('我是前台，我已收到你的消息：' + JSON.stringify(request));
    }
});
//09-捕捉到webcamscanqrcodeok消息，发送window消息
chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
    if (request.message == 'webcamscanqrcodeok') {
        window.postMessage({ "message": "webcamscanqrcodeok", "result": "" + request.result + "", "taskid": "" + request.taskid + ""  }, '*');

        sendResponse('我是前台，我已收到你的消息：' + JSON.stringify(request));
    }
});
function getCookie(name) {
    var cookies = document.cookie;
    var list = cookies.split("; ");          // 解析出名/值对列表

    for (var i = 0; i < list.length; i++) {
        var arr = list[i].split("=");          // 解析出名和值
        if (arr[0] == name)
            return decodeURIComponent(arr[1]);   // 对cookie值解码
    }
    return "";
}