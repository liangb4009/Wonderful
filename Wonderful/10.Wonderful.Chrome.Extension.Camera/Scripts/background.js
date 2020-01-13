//03-chrome捕捉到showwebcam消息
chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
    if (request.message == 'showwebcam') {
        //04-保存当前tabid
        chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
            chrome.storage.local.set({ 'webcamtabid': tabs[0].id });
        });
        //04-需要注册，如果关闭了当前tabid的tab，同步关闭当前tabid打开的所有windows
        chrome.tabs.onRemoved.addListener(function (tabId, removeInfo) {
            chrome.storage.local.get('webcamtabid', function (result) {
                if (result.webcamtabid == tabId) {
                    chrome.storage.local.get('webcamid', function (result) {
                        if (typeof (result.webcamid) != 'undefined') {
                            chrome.windows.remove(result.webcamid);
                            return;
                        }
                    })
                }
            })
        });

        //05-保存传入参数，存入localstorage
        var webcampara = {
            'taskid': request.taskid.toString(),
            'runmode': request.runmode.toString(),
            'filename': request.filename.toString(),
            'cameratype': request.cameratype.toString(),
            'machinename': request.machinename.toString(),
            'capturecount': request.capturecount.toString(),
            'capturefrequency': request.capturefrequency.toString(),
            'imagevalues': request.imagevalues,
            'language': request.language,
            'saveurl': request.saveurl
        };
        chrome.storage.local.set({ 'webcampara': webcampara });
        //06-创建操作窗体，保存窗体id
        chrome.storage.local.get('webcamid', function (result) {
            //--{下面通过判定window为undefined方法是不完美的
            //修改为在webcam.js，删除webcamid的本地存储，然后判断webcamid为undefined
            //chrome.windows.get(result.webcamid, null, function (w) {
            //    if (typeof (w) == 'undefined') {
            //        chrome.windows.create({
            //            url: 'main.html',
            //            type: 'popup',
            //            state: 'maximized'
            //        }, function (w) {
            //            chrome.storage.local.set({ 'webcamid': w.id });
            //        });
            //    }
            //});
            //--}
            //--{下面是完美的，只启动一个拍照窗口的方式
            if (typeof (result.webcamid) == 'undefined') {
                chrome.windows.create({
                    url: 'main.html',
                    type: 'popup',
                    state: 'maximized'
                }, function (w) {
                    chrome.storage.local.set({ 'webcamid': w.id });
                });
            }
            //--}
        })
        //07-发送消息给前台
        sendResponse('我是后台，我已收到你的消息：' + JSON.stringify(request));
    }
});



