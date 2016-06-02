$.ajaxSetup({
    cache: false //close AJAX cache
});

function openEditsysAdmin() {
    var openWin =
    new J.dialog({
        id: 'divEditsysAdmin',
        title: '修改个人资料',
        page: '/systemAdmin/SystemAdmin/EditCurr',
        width: 562,
        height: 225,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true //锁屏
    });
    openWin.ShowDialog();
}

function openChangPwd() {
    var openWin =
    new J.dialog({
        id: 'divChangePwd',
        title: '修改密码',
        page: '/systemAdmin/SystemAdmin/ChangePwd',
        width: 500,
        height: 188,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        resize: false,
        maxBtn: false,
        cover: true //锁屏
    });
    openWin.ShowDialog();
}