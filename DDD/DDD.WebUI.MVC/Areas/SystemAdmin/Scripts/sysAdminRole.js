$.ajaxSetup({
    cache: false //close AJAX cache
});
//打开窗口
global.define("IsChange", false); //用于判断是否改变
//刷新
function UpdatePFlag(ReDirectUrl) {
    if (global.IsChange)
        $("#divRoleList").load(ReDirectUrl);
    global.IsChange = false;
}
//添加
function openAddRole() {
    var openWin =
    new J.dialog({
        id: 'divaddRole',
        title: '添加角色',
        page: '/systemAdmin/AdminRole/Add/',
        width: 900,
        height: 600,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true //锁屏
    });
    openWin.ShowDialog();
}
//修改
function openUpdateRole(arid) {
    var openWin =
    new J.dialog({
        id: 'divupdateRole',
        title: '修改角色',
        page: '/systemAdmin/AdminRole/Edit/' + arid,
        width: 900,
        height: 600,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true, //锁屏
        onXclick: function () {
            UpdatePFlag("GetRoleList");
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag("GetRoleList");
        }
    });
    openWin.ShowDialog();
}
//设置
function openSetting(arid) {
    var openWin =
    new J.dialog({
        id: 'divSetting',
        title: '操作设置',
        page: '/systemAdmin/AdminRole/Setting/' + arid,
        width: 900,
        height: 600,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true, //锁屏
        onXclick: function () {
            UpdatePFlag("GetRoleList");
            openWin.cancel();
        },
        onCancel: function () {
            UpdatePFlag("GetRoleList");
        }
    });
    openWin.ShowDialog();
}
//删除
function roleDelete(id) {
    $.ajax({
        url: '/systemAdmin/AdminRole/Delete/' + id,
        type: 'post',
        beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
        success: function (data) {
            hideMessage(0);
            if (data.IsSuccess) {
                $("#divRoleList").load(data.ReDirectUrl);
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

function Modules(id) {
    var openWin =
    new J.dialog({
        id: 'divModules',
        title: '查看角色功能',
        page: '/systemAdmin/AdminRole/Modules/' + id,
        width: 350,
        height: 350,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true //锁屏
    });
    openWin.ShowDialog();
}