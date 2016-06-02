DG.addBtn('Sub', '修改', Sub);
function Sub() {
    if ($("#form0").valid())
        $("#form0").submit();
}
//修改成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        DG.curWin.showMessage("<span class='box-success'>成功</span>", content.TipMsg, true, false, false);
        DG.curWin.hideMessage(1);
        DG.cancel();
    }
    else {//程序异常、业务逻辑错误
        showMessage("<span class='box-error'>错误</span>", content.TipMsg, true, false, true);
    }
}