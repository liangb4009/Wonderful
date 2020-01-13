//02-window捕捉到showebscale消息，发送消息给chrome
window.addEventListener("message", function (e) {
    if (e.data.message == 'hautoproductoperate') {
        var appname = e.data.appname;
        var taskid = e.data.taskid;
        var DeviceSerialNumber = e.data.DeviceSerialNumber;
        var EQUIPMENT_ID = e.data.EQUIPMENT_ID;
        var HygrothermographAddress = e.data.HygrothermographAddress;
        var HygrothermographName = e.data.HygrothermographName;
        var HygrothermographPort = e.data.HygrothermographPort;
        var HygrothermographType = e.data.HygrothermographType;
        var StatusStr = e.data.StatusStr;
        //var scaletype = e.data.scaletype;
        //var scalename = e.data.scalename;
        //var scalefrequency = e.data.scalefrequency;
        //var scalecount = e.data.scalecount;
        //var scaleparams = e.data.scaleparams;
        //var scalevalues = e.data.scalevalues;
        chrome.extension.sendRequest(
            {
                appname: appname
            },
            function (response) {
                chrome.runtime.sendMessage(
                    response.appid,
                    {
                        message: 'hautoproductoperate',
                        taskid: taskid,
                        DeviceSerialNumber: DeviceSerialNumber,
                        EQUIPMENT_ID: EQUIPMENT_ID,
                        HygrothermographAddress: HygrothermographAddress,
                        HygrothermographName: HygrothermographName,
                        HygrothermographPort: HygrothermographPort,
                        HygrothermographType: HygrothermographType,
                        StatusStr: StatusStr
                    }, function (response) {
                        window.postMessage({ "message": "hautoproductoperateok", "response": response }, '*');
                    }
                );
            }
        );


       
    }
}, false);

