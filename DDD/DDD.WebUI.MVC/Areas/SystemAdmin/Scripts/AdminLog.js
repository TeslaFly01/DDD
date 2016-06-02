$.ajaxSetup({ cache: false });

global.define("CurrentIndex", 1);
global.define("UpdateFlag", false);

function WdateBegin() {
    return WdatePicker({ skin: 'whyGreen', maxDate: '#F{$dp.$D(\'txtEnd\')||\'%y-%M-%d\'}', readOnly: true });
}
function WdateEnd() {
    return WdatePicker({ skin: 'whyGreen', minDate: '#F{$dp.$D(\'txtBegin\')}', maxDate: '%y-%M-%d', readOnly: true });
}


function RefreshP() {
    if (global.UpdateFlag) {
        getPage(global.CurrentIndex);
        global.UpdateFlag = false;
    }
}

$(document).ready(function () {
    getPage(1);
})


function getPage(pageIndex) {
    var message = encodeURIComponent($("#hd_message").val());
    var username = encodeURIComponent($("#hd_username").val());
    var fromDate = $('#hd_begin').val();
    var toDate = $('#hd_end').val();
    $.ajax({
        type: "get",
        url: '/systemadmin/adminlog/List?pageIndex=' + pageIndex + '&OptContent=' + message + '&UserName=' + username + '&FromDate=' + fromDate + '&ToDate=' + toDate,
        cache: false,
        dataType: "html",
        beforeSend: function () {
            $("#load-s").show();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) { alert("网络错误，请稍后再试！"); /*alert(XMLHttpRequest.responseText);*/ },
        success: function (data) {
            $("#AdminLogList").html(data);
            window.scrollTo(0, 0);
            global.CurrentIndex = pageIndex;
            $("#load-s").hide();
        }
    });
}

function Search() {
    $('#hd_message').val($('#txtMessage').val());
    $('#hd_begin').val($('#txtBegin').val());
    $('#hd_end').val($('#txtEnd').val());
    $("#hd_username").val($('#txtUserName').val());
    getPage(1);
}

function ajaxDelete(id) {
    $.ajax({
        url: '/systemadmin/adminlog/Delete?id=' + id,
        type: 'get',
        beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
        success: function (data) {
            hideMessage(0);
            if (data.IsSuccess) {
                getPage(global.CurrentIndex);
                showMessage("<span class='box-success'>成功</span>", data.TipMsg, true, false, false);
                hideMessage(1);
                //setTimeout(hideMessage(), 500);
            }
            else {
                showMessage("<span class='box-error'>错误</span>", data.TipMsg, true, false, false);
            }
        },
        // error: function (XMLHttpRequest, textStatus, errorThrown) { alert(XMLHttpRequest.responseText); }
        error: function (data) { hideMessage(0); showMessage("<span class='box-error'>错误</span>", data.TipMsg, true, false, false); }
    });
}


function DeleteSome() {
    var DeleteIds = "";
    $("input[name='checkitem']").each(function () {
        if ($(this).prop("checked")) {
            DeleteIds += $(this).val() + "|";
        }
    });
    if (DeleteIds.length > 0) {
        if (!confirm("确定删除这些日志吗?")) return false;
        $.ajax({
            url: "/systemadmin/adminlog/DeleteSome?ids=" + DeleteIds,
            type: "post",
            beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
            dataType: "json",
            success: function (data) {
                hideMessage(0);
                if (data.IsSuccess) {
                    getPage(global.CurrentIndex);
                    showMessage("<span class='box-success'>成功</span>", data.TipMsg, true, false, false);
                    hideMessage(1);
                }
                else {
                    showMessage("<span class='box-error'>删除失败</span>", data.TipMsg, true, false, false);
                }
            },
            error: function () { hideMessage(0); showMessage("<span class='box-error'>删除失败</span>", "网络错误，请稍后再试！", true, false, false); }
        });

    }
    else {
        alert("请选择要删除的日志!");
    }
}

function DeleteBeforeDate() {
    var dt = $("#txt_ClearDateEnd").val();
    if (dt.length < 8) { alert("请选择有效的日期！"); return false; }
    if (!confirm("确定清除 " + dt + " 前的日志吗?")) return false;
    $.ajax({
        url: "/systemadmin/adminlog/CleareBeforeDate?dt=" + dt,
        type: "post",
        beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
        dataType: "json",
        success: function (data) {
            hideMessage(0);
            if (data.IsSuccess) {
                getPage(global.CurrentIndex);
                showMessage("<span class='box-success'>成功</span>", data.TipMsg, true, false, false);
                hideMessage(1);
            }
            else {
                showMessage("<span class='box-error'>删除失败</span>", data.TipMsg, true, false, false);
            }
        },
        error: function () { hideMessage(0); showMessage("<span class='box-error'>删除失败</span>", "网络错误，请稍后再试！", true, false, false); }
    });
}