$.ajaxSetup({
    cache: false
});

global.define("OrderTipDG", null);

$(function () {
    //验证权限后执行：
    var orderTipflag = $("#hid_corpSalesOrderTipFlag").val();
    if (orderTipflag == "True") {
        setInterval(OrderTipDg, 1000 * 30);
    }
});

function OrderTipDg() {
    $.ajax({
        url: "/CorpSalesOrderPayTip/getLastTip/",
        type: "GET",
        success: function (data) {
            if (data != null && data.IsSuccess) {
                var content = '<h3>订单【' + data.TObj.OrderNo + '】<br/>于' + data.TObj.PayTime + '支付成功！</h3>';
                openOrderTipDG(content);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
}

function openOrderTipDG(content) {
    if (global.OrderTipDG != null) { global.OrderTipDG.cancel(); }
    global.OrderTipDG =
                new J.dialog({
                    id: 'divWMallDG',
                    title: '订单交易提醒',
                    html: content,
                    width: 180,
                    height: 120,
                    rang: false,
                    iconTitle: false,
                    resize: false,
                    fixed: true,
                    left: 'right',
                    top: 'bottom',
                    maxBtn: false,
                    minBtn: false,
                    xButton: true,
                    cancelBtn: false,
                    dgOnLoad: function () {
                        global.OrderTipDG.addBtn('See', '立即查看', function () {
                            global.FrameTopper.getRequestId('/Order/CorpSalesOrder/');
                            //global.OrderTipDG.cancel();
                        });
                    }
                });
    global.OrderTipDG.ShowDialog();
}


function goHome() {
    getRequestId('/Home/Main/');
    $(".ll", window.parent.frames["left"].document).removeClass("cur");
}
function Refresh() {
    getRequestId(window.parent.document.getElementById("right").src);
}
function goHelp() {
    getRequestId('/CorpHelp/CorpHelp');
    $(".ll", window.parent.frames["left"].document).removeClass("cur");
}

var SizeFlag = true;
var reSize = function () {
    if (SizeFlag) {
        SizeFlag = false;
        StartGuideDG.reDialogSize(650, 0);
        StartGuideDG.SetPosition('right', 'top');
    } else {
        SizeFlag = true;
        StartGuideDG.reDialogSize(650, 510);
        StartGuideDG.SetPosition('center', 'center');
    }
};

var StartGuideDG = null;
function openStartGuide(Sindex) {
    StartGuideDG =
                new J.dialog({
                    id: 'divStartGuide',
                    title: '商家入门教学指引',
                    page: '/CorpHelp/CorpHelp/StartGuideIndex/?Sindex=' + Sindex,
                    width: 651,
                    height: 502,
                    rang: false,
                    iconTitle: false,
                    autoPos: true,
                    resize: true,
                    maxBtn: false,
                    minBtn: true,
                    xButton: true,
                    cancelBtnTxt: '关闭',
                    onMinSize: reSize,
                    dgOnLoad: function () {
                        StartGuideDG.SetIndex();
                    }
                });
    StartGuideDG.ShowDialog();
}
function openStartGuideVod() {
    var StartGuideVodDG =
                new J.dialog({
                    id: 'divStartGuideVod',
                    title: '商家入门教学指引-演示视频',
                    page: '/CorpHelp/CorpHelp/Vod',
                    width: 510,
                    height: 480,
                    rang: false,
                    iconTitle: false,
                    autoPos: true,
                    resize: false,
                    maxBtn: false,
                    minBtn: false,
                    xButton: true,
                    cancelBtnTxt: '关闭',
                    dgOnLoad: function () {
                        StartGuideVodDG.SetIndex();
                    }
                });
    StartGuideVodDG.ShowDialog();
}