chrome.extension.onRequest.addListener(
    function (request, sender, sendResponse) {

        //04-需要注册，如果关闭了当前tabid的tab，同步关闭当前tabid打开的所有windows
        chrome.tabs.onRemoved.addListener(function (tabId, removeInfo) {
            chrome.management.getAll(function (exInfoArray) {
                exInfoArray.forEach((data, index, array) => {
                    if (data.name == request.appname) {
                        chrome.runtime.sendMessage(
                            data.id,
                            {
                                management: chrome.management,
                                message: 'hautosproductclose'
                            }, function (response) {

                            }
                        );
                    }
                });
            });
        });


        chrome.management.getAll(function (exInfoArray) {
            exInfoArray.forEach((data, index, array) => {
                if (data.name == request.appname) {
                    sendResponse({ appid: data.id });
                }
            });
        });

    }
);
