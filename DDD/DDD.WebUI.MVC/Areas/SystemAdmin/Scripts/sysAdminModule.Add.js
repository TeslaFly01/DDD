//添加按钮
DG.addBtn('Sub', '添加', Sub);
function Sub() {
    if ($("#form0").valid())
        $("#form0").submit();
}
//添加成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        DG.curWin.global.IsChange = true; //是否更改
        DG.curWin.global.moduleFid = $("#FID").get(0).value; //刷新参数
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
//添加成功后的取消继续
function Undo(fid) {
    hideMessage(0);
    DG.curWin.UpdatePFlag("GetModuleList/" + fid);
    DG.cancel();
}