$(function () {
    $("#chkall").change(function () {
        $("input[name='moduleitem']").prop('checked', this.checked);
    });
});

function modulechange1(ipt) {
    $(ipt).parent().parent().parent().find("input[name='moduleitem']").prop('checked', $("#" + ipt.id).prop("checked"));
}
function modulechange2(ipt) {
    $(ipt).parent().parent().find("input[name='moduleitem']").prop('checked', $("#" + ipt.id).prop("checked"));
}

DG.addBtn('btnSub', '保存', Sub);
function Sub() {
    if ($("#form0").valid()) {
        var AddRoleIds = "";
        $("input[name='moduleitem']").each(function () {
            if ($(this).prop("checked")) {
                AddRoleIds += $(this).val() + "|";
            }
        });
        if (AddRoleIds.length > 0) {
            $("#ModuleIds").val(AddRoleIds);
            $("#form0").submit();
        } else {
            showMessage("<span class='box-error'>错误</span>", "你还未选择功能!", true, false, true);
        }
    }
}

//添加成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        DG.curWin.global.IsChange = true; //是否更改
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
function Undo() {
    hideMessage(0);
    DG.curWin.UpdatePFlag("GetRoleList");
    DG.cancel();
}