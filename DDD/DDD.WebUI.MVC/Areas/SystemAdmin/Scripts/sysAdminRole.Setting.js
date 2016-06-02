$.ajaxSetup({
    cache: false //close AJAX cache
});

//添加按钮
DG.addBtn('Sub', '确认', Sub);
//选择的功能
global.define("CMIDWeight", "");
function Sub() {
    $("input[name='moduleitem']").each(function () {
        if ($(this).prop("checked")) {
            global.CMIDWeight += $(this).val() + ",";
        } else {
            global.CMIDWeight += $(this).val().split('|')[0] + "|0,";
        }
    });

    $.ajax({ url: "/SystemAdmin/AdminRole/Setting/?arid=" + $("#hidARID").val() + "&CMIDWeight=" + global.CMIDWeight,
        type: "post", beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
        dataType: "json", success: function (result) {
            global.CMIDWeight = "";
            hideMessage(0);
            if (result.IsSuccess) {
                showMessage("<span class='box-success'>成功</span>", result.TipMsg, true, false, false);
                hideMessage(1);
            }
            else {//程序异常、业务逻辑错误
                DG.curWin.showMessage("<span class='box-error'>失败</span>", result.TipMsg, true, false, true);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            global.CMIDWeight = "";
            DG.curWin.hideMessage(0);
            DG.curWin.showMessage("<span class='box-error'>错误</span>", result.TipMsg, true, false, false);
        }
    });
}

$(function () {
    var TabNav = $(".tab-nav");
    var TabContent = $(".tab-content");
    var pTabTrigger = TabNav.find("a");

    pTabTrigger.click(function (e) {
        TabContent.hide();
        TabNav.find("li").removeClass("active");
        var activeID = $(this).attr("href");
        $(activeID).show();
        $(this).parent().addClass("active");
        e.preventDefault();
    });
    var amid = $("#hidAMID").val();
    var arid = $("#hidARID").val();
    getmoduleandaction(amid, arid);
});

function getmoduleandaction(amid, arid) {
    $.ajax({
        url: "/SystemAdmin/AdminRole/GetSecondModuleAndActions/",
        type: "get",
        data: { "fid": amid, "arid": arid },
        beforeSend: function () { $("#div_loading").show(); },
        complete: function () { $("#div_loading").hide(); },
        success: function (data) {
            $("#div_SettingModules").html(data).show();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络错误，请稍后再试！");
        }
    });
}