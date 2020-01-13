chrome.extension.onRequest.addListener(
    function (request, sender, sendResponse) {

        ////04-保存当前tabid
        chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
            chrome.storage.local.set({ 'webscaletabid': tabs[0].id });
        });
        //04-需要注册，如果关闭了当前tabid的tab，同步关闭当前tabid打开的所有windows
        chrome.tabs.onRemoved.addListener(function (tabId, removeInfo) {
            chrome.management.getAll(function (exInfoArray) {
                exInfoArray.forEach((data, index, array) => {
                    if (data.name == request.appname) {
                        chrome.runtime.sendMessage(
                            data.id,
                            {
                                management: chrome.management,
                                message: 'webscaleclose'
                            }, function (response) {

                            }
                        );
                    }
                });
            });
        });

        chrome.management.getAll(function (exInfoArray) {
            var appid = "";
            var extentionid = "";
            exInfoArray.forEach((data, index, array) => {
              
                if (data.name == request.appname) {
                    appid = data.id;
                }
                if (data.name == request.extentionname) {
                    extentionid = data.id;
                }
            });
            chrome.storage.local.set({ 'webscaleextentionid': extentionid });
            sendResponse({ appid: appid, extentionid: extentionid });
        });
        
    }
);





//一次性请求与响应模式:
chrome.runtime.onMessageExternal.addListener(
    function (request, sender, sendResponse) {
        try {
            chrome.storage.local.get('webscaletabid', function (result) {
                var webscaletabid = result.webscaletabid;
                chrome.tabs.sendMessage(webscaletabid, { mssg: "SendIt", taskid: request.taskid }, function (response) {
                    console.log(response);
                });
            });
        }
        catch (error) {
            console.log(error);
        }
    });

