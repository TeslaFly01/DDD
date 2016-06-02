DG.addBtn('Sub', '修改', Sub);
function Sub() {
    if ($("#form0").valid())
        $("#form0").submit();
}
//修改成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        DG.iWin("divRightsManager").global.IsChange = true; //是否更改
        DG.iWin("divRightsManager").showMessage("<span class='box-success'>成功</span>", content.TipMsg, true, false, false);
        DG.iWin("divRightsManager").hideMessage(1);
        if (DG.iDG("divRightsManager")) {
            DG.iWin("divRightsManager").UpdatePFlag("/systemadmin/adminaction/GetActionList/" + $("#AMID").val());
            DG.cancel();
        } else {
            alert("父页面丢失!");
        }
    }
    else {//程序异常、业务逻辑错误
        showMessage("<span class='box-error'>错误</span>", content.TipMsg, true, false, true);
    }
}