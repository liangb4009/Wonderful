window.data = {
    rtnmsg: '',
    timestamp: new Date().getTime()
};
var serialPort = [];

chrome.runtime.onConnectExternal.addListener(
    function (port) {
        var portIndex = getGUID();
        serialPort[portIndex] = port;
        port.postMessage({ header: "guid", guid: portIndex });
        port.onDisconnect.addListener(
            function () {
                serialPort.splice(portIndex, 1);
                console.log("Web page closed guid " + portIndex);
            }
        );

        console.log("New web page with guid " + portIndex);
    }
);

chrome.app.runtime.onLaunched.addListener(function () {
    var main_window = chrome.app.window.get('main');
    if (main_window) {
        main_window.show();
    }
    else {
        chrome.app.window.create('main.html', {
            id: 'main',
            state: 'maximized',
            resizable: true
        });
    }
});
chrome.runtime.onMessageExternal.addListener(function (request, sender, sendResponse) {
    if (request.message == 'webscaleoperate') {

        var main_window = chrome.app.window.get('main');
        if (main_window) {
            main_window.show();
            window.data.rtnmsg = 'main.html is existed!';
            sendResponse(window.data);
        }
        else {
            chrome.app.window.create('main.html', {
                id: 'main',
                width: 1000,
               height:2000,
                'frame': 'none'
            });
            window.data.rtnmsg = 'main.html is created';
            //03-∑µªÿ–≈œ¢
            sendResponse(window.data);
            var webscaleshowinfo = {
                'taskid': request.taskid,
                'scaletype': request.scaletype,
                'module': request.module,
                'moduletype': request.moduletype,
                'scalename': request.scalename,
                'scalefrequency': request.scalefrequency,
                'scalecount': request.scalecount,
                'scaleparams': request.scaleparams,
                'scalevalues': request.scalevalues,
                'webscaleextentionid': request.webscaleextentionid,
                'scalesaveurl': request.scalesaveurl
            };
            chrome.storage.local.set({ 'webscaleshowinfo': webscaleshowinfo });
        }

        


    }
    if (request.message == 'updateinfo') {
        var main_window = chrome.app.window.get('main');
        if (main_window) {
            var showinfo = {
                'type': "cut",
                'm': "m",
                'amc': "amc",
                'cupboardno': "01",
                'surplusquality': 10,
                'batchquality': 100,
                'headerid': "",
                'batchid': ""
            };
            chrome.storage.local.set({ 'webscaleshowinfo': showinfo });
            window.data.rtnmsg = 'main.html is updated!'
            sendResponse(window.data);
        }
    }
    if (request.message == 'webscaleclose') {
        try {
            chrome.storage.local.get("serialportConnectionID", function (result) {
                if (result && result.serialportConnectionID && result.serialportConnectionID != '') {
                    chrome.serial.flush(result.serialportConnectionID, function (result1) {
                        console.log(result1);
                    });

                    chrome.serial.disconnect(result.serialportConnectionID, function () {

                    });
                    chrome.storage.local.remove('serialportConnectionID', function () { });
                }

            });
        }
        catch (error) {
            window.data.rtnmsg += error;
        }

        var main_window = chrome.app.window.get('main');
        if (main_window) {
            if (main_window.close) {
                main_window.close();   
            }               
        }     
        window.data.rtnmsg = 'main.html is closed!';
        sendResponse(window.data);
    }
});

