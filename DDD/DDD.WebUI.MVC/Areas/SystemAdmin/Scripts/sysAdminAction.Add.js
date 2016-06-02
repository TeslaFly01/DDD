//添加按钮
DG.addBtn('Sub', '添加', Sub);
function Sub() {
    if ($("#form0").valid()) {
        $("#form0").submit();
    }
}
//添加成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        DG.iWin("divRightsManager").global.IsChange = true; //是否更改
        DG.iWin("divRightsManager").global.actionAmid = $("#AMID").val(); //刷新参数
        showMessage("<span class='box-success'>成功</span>", content.TipMsg, false, false, true);
    }
    else {//程序异常、业务逻辑错误
        showMessage("<span class='box-error'>错误</span>", content.TipMsg, true, false, true);
    }
}
//继续添加
function Goon() {
    hideMessage(0);
    window.location = window.location;
}
//添加成功取消继续
function Undo(amid) {
    hideMessage(0);
    if (DG.iDG("divRightsManager")) {
        DG.iWin("divRightsManager").UpdatePFlag("/systemadmin/adminaction/GetActionList/" + amid);
        DG.cancel();
    } else {
        alert("父页面丢失!");
    }
}