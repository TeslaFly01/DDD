$.ajaxSetup({
    cache: false //close AJAX cache
});
global.define("CurrentIndex", 1); //分页的当前页

//加载
$(document).ready(function () {
    getPage(1);
})
//分页
function getPage(pageIndex) {
    var txtuid = encodeURIComponent($("#srarchName").val());
    var txtname = encodeURIComponent($("#srarchNickName").val());
    var selrole = $("#srarchRole").val();
    $.ajax({
        url: "/systemAdmin/SystemAdmin/GetSysadminList/?pageIndex=" + pageIndex + "&arid=" + selrole + "&nam=" + txtuid + "&nknam=" + txtname, type: "get", beforeSend: function () { $("#load-s").show(); },
        success: function (data) {
            $("#divSystemAdminList").html(data);
            window.scrollTo(0, 0);
            global.CurrentIndex = pageIndex;
            $("#load-s").hide();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#load-s").show(); /*$("#").html(XMLHttpRequest.responseText);*/
            showMessage("<span class='box-error'>错误</span>", "网络错误,请稍后再试!", true, false, false);
            $("#load-s").hide();
        }
    });
}
//搜索
function sysAdminSearch() {
    $("#srarchName").val($("#txtName").val());
    $("#srarchNickName").val($("#txtNickName").val());
    $("#srarchRole").val($("#RoleSearch").get(0).value);
    getPage(1);
}
//启用、禁用
global.define("EnbIds", "");
function IsEnable(use) {
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
        alert("请选择要操作的数据项!");
        return;
    }
    if (confirm(cmstr)) {
        $.ajax({
            url: "Enable/?ids=" + global.EnbIds + "&isEnable=" + enb,
            type: "post", beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); }, dataType: "json", success: function (result) {
                hideMessage(0);
                if (result.IsSuccess) {
                    getPage(global.CurrentIndex);
                    showMessage("<span class='box-success'>成功</span>", result.TipMsg, true, false, false);
                    hideMessage(1);
                }
                else {
                    showMessage("<span class='box-error'>失败</span>", result.TipMsg, true, false, false);
                }
            },
            error: function () { hideMessage(0); showMessage("<span class='box-error'>错误</span>", "网络错误，请稍后再试！", true, false, false); }
        });
    }
}
//打开窗口
global.define("IsChange", false); //用于判断是否改变
//刷新
function UpdatePFlag(isAdd) {
    if (global.IsChange) {//此判断是为了添加成功后直接跳转到第一页
        if (isAdd != null && isAdd) {
            getPage(1);
        } else {
            getPage(global.CurrentIndex);
        }
    }
    global.IsChange = false;
}
//添加
function openAddSysAdmin() {
    var openWin =
    new J.dialog({
        id: 'divaddSysAdmin',
        title: '添加系统管理员',
        page: '/systemAdmin/SystemAdmin/Add',
        width: 562,
        height: 380,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true, //锁屏
        onXclick: function () {
            UpdatePFlag(true);
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag(true);
        }
    });
    openWin.ShowDialog();
}
//修改
function openUpdateSysAdmin(said) {
    var openWin =
    new J.dialog({
        id: 'divupdateSysAdmin',
        title: '修改系统管理员',
        page: '/systemAdmin/SystemAdmin/Edit/' + said,
        width: 562,
        height: 340,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true, //锁屏
        onXclick: function () {
            UpdatePFlag();
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag();
        }
    });
    openWin.ShowDialog();
}
//删除
function adminDelete(id) {
    $.ajax({
        url: '/systemadmin/SystemAdmin/Delete/' + id,
        type: 'post',
        beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
        success: function (data) {
            hideMessage(0);
            if (data.IsSuccess) {
                getPage(global.CurrentIndex);
                showMessage("<span class='box-success'>成功</span>", data.TipMsg, true, false, false);
                hideMessage(1);
            }
            else {
                showMessage("<span class='box-error'>失败</span>", data.TipMsg, true, false, false);
            }
        },
        // error: function (XMLHttpRequest, textStatus, errorThrown) { alert(XMLHttpRequest.responseText); }
        error: function (data) { hideMessage(0); showMessage("<span class='box-error'>错误</span>", data.TipMsg, true, false, false); }
    });
}
