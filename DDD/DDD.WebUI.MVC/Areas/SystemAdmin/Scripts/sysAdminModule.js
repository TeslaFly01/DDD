$.ajaxSetup({
    cache: false //close AJAX cache
});
//禁用、启用 批量删除
global.define("DeleteIds", "");
global.define("EnbIds", "");
function IsEnableDelete(Action, use) {
    var selectfid = $("#selectSearch").get(0).value;
    if (use == "del") //删除
    {
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
        if (confirm("确定删除这些吗?下属所有操作权限将同时被删除!")) {
            $.ajax({
                url: Action + "/?ids=" + global.DeleteIds + "&fid=" + selectfid,
                type: "post", beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); }, dataType: "json", success: function (result) {
                    hideMessage(0);
                    if (result.IsSuccess) {
                        $("#divModuleList").load(result.ReDirectUrl);
                        showMessage("<span class='box-success'>成功</span>", result.TipMsg, true, false, false);
                        hideMessage(1);
                    }
                    else {
                        showMessage("<span class='box-error'>操作失败</span>", result.TipMsg, true, false, false);
                    }
                },
                error: function () { hideMessage(0); showMessage("<span class='box-error'>错误</span>", "网络错误，请稍后再试！", true, false, false); }
            });
        }
    } else {
        var enb = false;
        var cmstr = "";
        if (use == "eby")//启用
        {
            enb = true;
            cmstr = "确定要启用这些吗?";
        }
        else if (use == "ebn")//禁用
        {
            enb = false;
            cmstr = "确定要禁用这些吗?";
        }
        else {
            alert("找不到的操作功能!");
            return;
        }
        global.EnbIds = "";
        $("input[name='checkitem']").each(function () {
            if ($(this).prop("checked")) {
                global.EnbIds += $(this).val() + "|";
            }
        });
        if (global.EnbIds.length == 0) {
            alert("请选择要操作的数据项！");
            return;
        }
        if (confirm(cmstr)) {
            $.ajax({
                url: Action + "/?ids=" + global.EnbIds + "&isEnable=" + enb + "&fid=" + selectfid,
                type: "post", beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); }, dataType: "json", success: function (result) {
                    hideMessage(0);
                    if (result.IsSuccess) {
                        $("#divModuleList").load(result.ReDirectUrl);
                        showMessage("<span class='box-success'>成功</span>", result.TipMsg, true, false, false);
                        hideMessage(1);
                    }
                    else {
                        showMessage("<span class='box-error'>操作失败</span>", result.TipMsg, true, false, false);
                    }
                },
                error: function () { hideMessage(0); showMessage("<span class='box-error'>错误</span>", "网络错误，请稍后再试！", true, false, false); }
            });
        }
    }
}
//打开窗口
global.define("IsChange", false); //用于判断是否改变
global.define("moduleFid", 0); //记录当前操作的amid
//刷新
function UpdatePFlag(ReDirectUrl) {
    if (global.IsChange)
        $("#divModuleList").load(ReDirectUrl);
    global.IsChange = false;
    global.moduleFid = 0;
}
//添加
function openAddModule() {
    var openWin =
    new J.dialog({
        id: 'divaddModule',
        title: '添加功能模版',
        page: '/systemAdmin/AdminModule/AddModule/' + $("#selectSearch").get(0).value,
        width: 562,
        height: 286,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true, //锁屏
        onXclick: function () {
            UpdatePFlag("GetModuleList/" + global.moduleFid);
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag("GetModuleList/" + global.moduleFid);
        }
    });
    openWin.ShowDialog();
}
//修改
function openUpdateModule(mid) {
    var openWin =
    new J.dialog({
        id: 'divUpdateModule',
        title: '修改功能模版',
        page: '/systemAdmin/AdminModule/EditModule/' + mid,
        width: 562,
        height: 286,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true, //锁屏
        onXclick: function () {
            UpdatePFlag("GetModuleList/" + global.moduleFid);
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag("GetModuleList/" + global.moduleFid);
        }
    });
    openWin.ShowDialog();
}
//操作权限
function openRightsManager(Amid) {
    var openWin =
    new J.dialog({
        id: 'divRightsManager',
        title: '操作权限管理',
        page: '/systemAdmin/AdminAction/Index/' + Amid,
        width: 800,
        height: 500,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        btnBar: false, //按钮栏
        cover: true//锁屏
    });
    openWin.ShowDialog();
}
//删除成功
function DeleteSuccess(result) {
    hideMessage(0);
}