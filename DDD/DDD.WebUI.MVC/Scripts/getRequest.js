//组合请求
function getRequestId(requestID) {
    var frame = window.parent.document.getElementById("right"); //frame
    var loading = window.top.document.getElementById("frmloading"); //进度条
    loading.style.left = ((window.screen.width - 140) / 2) + "px";
    loading.style.top = ((window.screen.height - 38) / 2) + "px";
    loading.style.display = "block";
    frame.src = requestID;
    restoreViewState();
}

function restoreViewState() {
    var frame0 = window.parent.document.getElementById("right");
    var loading0 = window.top.document.getElementById("frmloading");

    if (frame0.attachEvent) {
        frame0.attachEvent("onload", function() {
            loading0.style.display = "none";
        });

    } else {
        frame0.onload = function() {
            loading0.style.display = "none";
        };
    }
}

function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = { };
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

