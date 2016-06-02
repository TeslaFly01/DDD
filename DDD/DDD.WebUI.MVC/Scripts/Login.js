var MsgDg = null;
function showMessage(tit, mess, showXBut, showloadpic, showCover) {
    MsgDg = null;
    var newmess = "";
    if (showloadpic) newmess = "<div align='center' style='padding-top:10px;'><div style='margin-left:28px;float:left;'><img src='/Content/images/loading.gif' align='center'   /></div><div style='margin-top:2px; margin-left:5px; float:left;'>" + mess + "</div></div>";
    else newmess = "<div align='center' style='padding-top:10px;'>" + mess + "</div>";
    MsgDg = new J.dialog({
        id: 'msgModal',
        titleBar: true,
        title: tit,
        width: 200,
        height: 110,
        rang: true,
        autoPos: true,
        btnBar: false,
        cover: showCover,
        maxBtn: false,
        minBtn: false,
        iconTitle: false,
        xButton: showXBut,
        skin: 'facebook',
        bgcolor: '#000',
        opacity: 0.2,
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

function AjaxStart() {
    showMessage("", "正在请求，请等待！", false, true, true);
}
function AjaxStop() {
    hideMessage(0);
}
function AjaxFailure(content) {
    hideMessage(0);
    $('#imgcode').attr("src", "/Home/VerifyImage?t=" + Math.random());
    $("#ccode").val("");
    showMessage("<span class='box-error'>登录失败</span>", $.parseJSON(content.responseText).TipMsg, true, false, false);
    hideMessage(8);
}
function resetBtn(fm) {
    fm.reset();
    return false;
}

$(function () {
    $("#SAName").focus(function () {
        $(".user-icon").css("left", "-48px");
    });
    $("#SAName").blur(function () {
        $(".user-icon").css("left", "0px");
    });

    $("#SAPwd").focus(function () {
        $(".pass-icon").css("left", "-48px");
    });
    $("#SAPwd").blur(function () {
        $(".pass-icon").css("left", "0px");
    });
    $("#ccode").focus(function () {
        $(".pin-icon").css("left", "-48px");
    });
    $("#ccode").blur(function () {
        $(".pin-icon").css("left", "0px");
    });
    $('input:text:first').focus();
    $('#ccode').poshytip({
        className: 'tip-yellowsimple',
        showOn: 'focus',
        alignTo: 'target',
        alignX: 'center',
        offsetX: 0,
        offsetY: 5
    });
    var loginform = $("#form0");
    loginform.keydown(function (e) {
        var key = e.which;
        if (key == 13) {
            return false;
        }
    });
    $('#imgcode').attr("src", "/Home/VerifyImage?t=" + Math.random());
    $("#ccode").keydown(function (e) {
        var key = e.which;
        if (key == 13) {
            this.blur();
            if (loginform.valid()) { loginform.submit(); }
        }
    });
    $("#imgcode").click(function () {
        $(this).attr("src", "/Home/VerifyImage?t=" + Math.random());
        $('#ccode').val('');
        return false;
    });
    $.preloadImages("/Scripts/lhgdialog/skins/facebook/lhgdg_bg.png", "/Content/images/nav_fhui1.png", "/Content/images/nav_help1.png", "/Content/images/nav_home1.png", "/Content/images/nav_refresh1.png", "/Content/images/nav_user1.png");
});

function AjaxSuccess(content) {

    if (content.IsSuccess) {
        $("input").prop("disabled", true);
        window.location = "/Home/Index";
        hideMessage(0);
    }
    else {//程序异常、业务逻辑错误
        hideMessage(0);
        if (content.ReDirectUrl != null && content.ReDirectUrl != undefined && content.ReDirectUrl.length > 0) {
            window.location.href = content.ReDirectUrl;
            return;
        }
        $('#imgcode').attr("src", "/Home/VerifyImage?t=" + Math.random());
        $("#ccode").val("");
        showMessage("<span class='box-error'>登录失败</span>", content.TipMsg, true, false, false);
        hideMessage(8);
    }
}

function FindPass() {
    DG = new J.dialog({
        id: 'FindPassDG',
        title: '重设密码',
        page: '/Home/FindPassword',
        width: 510,
        height: 320,
        rang: true,
        autoPos: true,
        btnBar: true,
        cover: true,
        maxBtn: false,
        minBtn: false,
        iconTitle: false,
        xButton: true,
        onXclick: RefreshP,
        onCancel: RefreshP
    });
    DG.ShowDialog();
}

function RefreshP() {
    if (DG.dg) DG.cancel();
}