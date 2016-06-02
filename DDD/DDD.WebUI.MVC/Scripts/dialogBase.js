var DG = frameElement.lhgDG;

var MsgDg = null;
function showMessage(tit, mess, showXBut, showloadpic, showCover) {
    MsgDg = null;
    var newmess = "";
    if (showloadpic) newmess = "<div align='center' style='padding-top:10px;'><div style='margin-left:28px;float:left;'><img src='/Content/images/loading.gif' align='center'   /></div><div style='margin-top:2px; margin-left:5px; float:left;'>" + mess + "</div></div>";
    else newmess = "<div align='center' style='padding-top:10px;'>" + mess + "</div>";
    MsgDg = new DG.curWin.J.dialog({
        id: 'msgModal',
        titleBar: true,
        title: tit,
        width: 200,
        height: 115,
        rang: true,
        autoPos: true,
        btnBar: false,
        cover: showCover,
        maxBtn: false,
        minBtn: false,
        iconTitle: false,
        xButton: showXBut,
        skin: 'facebook',
        parent: DG,
        html: newmess
    });
    MsgDg.ShowDialog();
}

function showMessage2(tit, mess, showXBut, showloadpic, showCover, width) {
    MsgDg = null;
    width = isNaN(width) ? 200 : width;
    var newmess = "";
    if (showloadpic) newmess = "<div align='center' style='padding-top:10px;'><div style='margin-left:28px;float:left;'><img src='/Content/images/loading.gif' align='center'   /></div><div style='margin-top:2px; margin-left:5px; float:left;'>" + mess + "</div></div>";
    else newmess = "<div align='center' style='padding-top:10px;'>" + mess + "</div>";
    MsgDg = new DG.curWin.J.dialog({
        id: 'msgModal',
        titleBar: true,
        title: tit,
        width: width,
        height: 115,
        rang: true,
        autoPos: true,
        btnBar: false,
        cover: showCover,
        maxBtn: false,
        minBtn: false,
        iconTitle: false,
        xButton: showXBut,
        skin: 'facebook',
        parent: DG,
        html: newmess
    });
    MsgDg.ShowDialog();
}

function hideMessage(t) {
    if (t == 0) MsgDg.cancel();
    else MsgDg.closeTime(t);
}


function refreshPage() {
    window.location.href = "./";
}

function AjaxStart() { showMessage("", "正在请求，请等待！", false, true, true) }
function AjaxStop() { hideMessage(0) }
function AjaxFailure(content) {
    hideMessage(0);
    showMessage("错误", content.responseText, true, false, false);
}