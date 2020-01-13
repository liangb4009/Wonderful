window.data = {
    rtnmsg: '',
    timestamp: new Date().getTime()
};



//chrome.app.runtime.onLaunched.addListener(function () {
    
//    chrome.storage.local.get('hautodeviceinfo', function (result) {
//        var currentdeviceinfo = result;
//        var main_window = chrome.app.window.get('hautoproductmain' + currentdeviceinfo.hautodeviceinfo.EQUIPMENT_ID);
//        if (main_window) {
//            main_window.show();
//        }
//        else {
//            chrome.app.window.create('hautoproductmain.html', {
//                id: 'hautoproductmain' + currentdeviceinfo.hautodeviceinfo.EQUIPMENT_ID,
//                state: 'maximized',
//                resizable: true
//            });
//        }
//    });
    
   
//});


chrome.runtime.onMessageExternal.addListener(function (request, sender, sendResponse) {
    if (request.message == 'hautoproductoperate') {
        try {
            var hautodeviceinfo = {
                'taskid': request.taskid,
                'DeviceSerialNumber': request.DeviceSerialNumber,
                'EQUIPMENT_ID': request.EQUIPMENT_ID,
                'HygrothermographAddress': request.HygrothermographAddress,
                'HygrothermographName': request.HygrothermographName,
                'HygrothermographPort': request.HygrothermographPort,
                'HygrothermographType': request.HygrothermographType,
                'Status': request.Status
            };
            chrome.storage.local.remove('hautodeviceinfo', function (info) {
                window.data.rtnmsg += info;
            });
            chrome.storage.local.set({ 'hautodeviceinfo': hautodeviceinfo });
          
        }
        catch (error) {
            sendResponse(error);
        }

        var main_window = chrome.app.window.get('hautoproductmain' + request.EQUIPMENT_ID);
        if (main_window) {
            main_window.show();
            window.data.rtnmsg = 'hautoproductmain.html' + request.EQUIPMENT_ID+' is existed!';
            sendResponse(window.data);
        }
        else {
            chrome.app.window.create('hautoproductmain.html', {
                'id': 'hautoproductmain' + request.EQUIPMENT_ID,
                'state': 'maximized',              
                'frame': 'none'
            });
            window.data.rtnmsg = 'hautoproductmain' + request.EQUIPMENT_ID + '.html is created' + request.DeviceSerialNumber ;
          
            //03-返回信息
            sendResponse(window.data);          
        }
    }
    //if (request.message == 'hautosproductclose') {

    //}
    //if (request.message == 'updateinfo') {
    //    var main_window = chrome.app.window.get('hautoproductmain');
    //    if (main_window) {
    //        var showinfo = {
    //            'type': "cut",
    //            'm': "m",
    //            'amc': "amc",
    //            'cupboardno': "01",
    //            'surplusquality': 10,
    //            'batchquality': 100,
    //            'headerid': "",
    //            'batchid': ""
    //        };
    //        chrome.storage.local.set({ 'showinfo': showinfo });
    //        window.data.rtnmsg = 'hautoproductmain.html is updated!'
    //        sendResponse(window.data);
    //    }
    //}
});



