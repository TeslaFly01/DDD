//得到树形选中数据
global.define("RoleIds", "");
//添加按钮
DG.addBtn('Sub', '修改', Sub);
function Sub() {
    if ($("#form0").valid()) {
        global.RoleIds = "";
        $("input[name='checkroleitem']").each(function () {
            if ($(this).prop("checked")) {
                global.RoleIds += $(this).val() + "|";
            }
        });
        if (global.RoleIds.length > 0) {
            $("#CheckRoleIds").val(global.RoleIds);
            $("#form0").submit();
        } else {
            alert("你还未选择角色!");
        }
    }
}
//修改成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        DG.curWin.global.IsChange = true; //是否更改
        DG.curWin.showMessage("<span class='box-success'>成功</span>", content.TipMsg, true, false, false);
        DG.curWin.hideMessage(1);
        DG.curWin.UpdatePFlag();
        DG.cancel();
    }
    else {//程序异常、业务逻辑错误
        showMessage("<span class='box-error'>错误</span>", content.TipMsg, true, false, true);
    }
}