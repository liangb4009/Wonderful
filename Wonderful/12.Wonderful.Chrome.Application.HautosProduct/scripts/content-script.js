////02-window捕捉到showebhautoproduct消息，发送消息给chrome
//window.addEventListener("message", function (e) {
//    if (e.data.message1 == 'hautoproductoperate') {
//        var appid = e.data.appid;
//        var taskid = e.data.taskid;
//        var scaletype = e.data.scaletype;
//        var scalename = e.data.scalename;
//        var scalefrequency = e.data.scalefrequency;
//        var scalecount = e.data.scalecount;
//        var scaleparams = e.data.scaleparams;
//        var scalevalues = e.data.scalevalues;
//        chrome.runtime.sendMessage(
//            appid,
//            {
//                message: 'hautoproductoperate',
//                taskid: taskid,
//                scaletype: scaletype,
//                scalename: scalename,
//                scalefrequency: scalefrequency,
//                scalecount: scalecount,
//                scaleparams: scaleparams,
//                scalevalues: scalevalues
//            }, function (response) {
//                window.postMessage({ "message": "hautoproductoperateok", "response": response }, '*');
//            }
//        );
//    }
//}, false);



