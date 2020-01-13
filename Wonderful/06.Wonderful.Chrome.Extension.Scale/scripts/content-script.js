//02-window捕捉到showebscale消息，发送消息给chrome
window.addEventListener("message", function (e) {
    if (e.data.message == 'webscaleoperate') {
        var appname = e.data.appname;
        var extentionname = e.data.extentionname;
        var taskid = e.data.taskid;
        var scaletype = e.data.scaletype;
        var scalename = e.data.scalename;
        var scalefrequency = e.data.scalefrequency;
        var scalecount = e.data.scalecount;
        var scaleparams = e.data.scaleparams;
        var scalevalues = e.data.scalevalues;
        var module = e.data.module;
        var moduletype = e.data.moduletype;
        var scalesaveurl = e.data.scalesaveurl
        chrome.extension.sendRequest(
            {
                appname: appname,
                extentionname: extentionname
            },
            function (res) {

                var appid = res.appid;               
                var extentionid = res.extentionid;
                chrome.runtime.sendMessage(
                    res.appid,
                    {
                        management: chrome.management,
                        message: 'webscaleoperate',
                        module: e.data.module,
                        moduletype: e.data.moduletype,
                        taskid: taskid,
                        scaletype: scaletype,
                        scalename: scalename,
                        scalefrequency: scalefrequency,
                        scalecount: scalecount,
                        scaleparams: scaleparams,
                        scalevalues: scalevalues,
                        webscaleextentionid: extentionid,
                        scalesaveurl: scalesaveurl
                    }, function (response) {
                        window.postMessage({ "message": "webscaleoperateok", "response": response, "taskid": taskid }, '*');
                    }
                );
            }
        );
    }
  
}, false);

////09-捕捉到webscalescanok消息，发送window消息

chrome.extension.onMessage.addListener(function (msg, sender, sendResponse) {
    try {
        if (msg.mssg == 'SendIt') {
            window.postMessage({ "message": "scalescan_complete", "taskid": msg.taskid, "response": "response" }, '*');
           
        }
        sendResponse({ "retMsg": "ok" });
    }
    catch (error) {
        console.log(error);
    }
});