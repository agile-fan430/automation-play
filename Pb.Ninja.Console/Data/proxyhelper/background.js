var Global = {
    currentProxyAouth: {
        username: 'lum-customer-hl_637ef71b-zone-static_res',
        password: 'ujf5m5wq32va'
    }
}

var userString = navigator.userAgent.split('$PC$');
if (userString.length > 1) {
    var credential = userString[1];
    var userInfo = credential.split(':');
    if (userInfo.length > 1) {
        Global.currentProxyAouth = {
            username: userInfo[0],
            password: userInfo[1]
        }
    }
}

chrome.webRequest.onAuthRequired.addListener(
    function(details, callbackFn) {
        console.log('onAuthRequired >>>: ', details, callbackFn);
        callbackFn({
            authCredentials: Global.currentProxyAouth
        });
    }, {
        urls: ["<all_urls>"]
    }, ["asyncBlocking"]);


chrome.runtime.onMessage.addListener(
    function(request, sender, sendResponse) {
        console.log('Background recieved a message: ', request);

        POPUP_PARAMS = {};
        if (request.command && requestHandler[request.command])
            requestHandler[request.command](request);
    }
);