$.ajaxSetup({
    cache: false //close AJAX cache
});
//批量删除
global.define("DeleteIds", "");
function DeleteList(Aid) {
    global.DeleteIds = "";
    $("input[name='checkitem']").each(function () {
        if ($(this).prop("checked")) {
            global.DeleteIds += $(this).val() + "|";
        }
    });
    if (global.DeleteIds.length == 0) {
        alert("请选择要操作的数据项！");
        return;
    }
    if (confirm("确定删除这些吗?")) {
        $.ajax({ url: "/systemadmin/adminaction/DeleteActionList/?ids=" + global.DeleteIds + "&amid=" + Aid,
            type: "post", beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
            dataType: "json", success: function (result) {
                if (result.IsSuccess) {
                    $("#divActionList").load(result.ReDirectUrl);
                    hideMessage(1);
                }
                else {
                    hideMessage(0);
                    showMessage("<span class='box-error'>失败</span>", result.TipMsg, true, false, false);
                }
            },
            error: function () { hideMessage(0); showMessage("<span class='box-error'>错误</span>", "网络错误，请稍后再试！", true, false, false); }
        });
    }
}
//打开窗口
global.define("IsChange", false); //用于判断是否改变
global.define("actionAmid", 0); //记录当前操作的fid
//刷新
function UpdatePFlag(ReDirectUrl) {
    if (global.IsChange)
        $("#divActionList").load(ReDirectUrl);
    global.IsChange = false;
    global.actionAmid = 0;
}
//添加
function openAddAction(Amid) {
    var openWin =
    new DG.curWin.J.dialog({
        id: 'divaddAction',
        title: '添加操作权限',
        page: '/systemAdmin/AdminAction/AddAction/' + Amid,
        width: 562,
        height: 255,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        cover: true, //锁屏
        resize: false,
        maxBtn: false,
        parent: DG,
        onXclick: function () {
            UpdatePFlag("/systemadmin/adminaction/GetActionList/" + global.actionAmid);
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag("/systemadmin/adminaction/GetActionList/" + global.actionAmid);
        }
    });
    openWin.ShowDialog();
}
//修改
function openUpdateAction(Aaid) {
    var openWin =
    new DG.curWin.J.dialog({
        id: 'divUpdateAction',
        title: '修改操作权限',
        page: '/systemAdmin/AdminAction/EditAction/' + Aaid,
        width: 562,
        height: 255,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        cover: true, //锁屏
        resize: false,
        maxBtn: false,
        parent: DG,
        onXclick: function () {
            UpdatePFlag("/systemadmin/adminaction/GetActionList/" + global.actionAmid);
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag("/systemadmin/adminaction/GetActionList/" + global.actionAmid);
        }
    });
    openWin.ShowDialog();
}
//删除成功
function DeleteSuccess() {
    hideMessage(0);
    showMessage("<span class='box-success'>成功</span>", "删除成功!", true, false, false);
    hideMessage(1);
}