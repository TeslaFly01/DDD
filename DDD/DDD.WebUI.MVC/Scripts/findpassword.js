DG.addBtn('btnSub', '确定', SubAdd);
function SubAdd() {
    if ($("input[name=findType][type=hidden]").length <= 0) {
        if ($("input[name=findType]:checked").val() == 1) {
            $("#Mobile").val("13800138000");
        } else {
            $("#Email").val("sonctech@gmail.com");
        }
    }
    if ($("#form0").valid()) {
        $("#form0").submit();
    }
}

$(function () {
    $("#imgcode").attr("src", "/core/verifyimage/getpinimage?sname=ValidCode&t=" + Math.random());
    $("#imgcode").click(function () {
        $(this).attr("src", $(this).attr("src").split("&")[0] + "&t=" + Math.random());
        $('#ValidCode').val('');
        return false;
    });
    $('#ValidCode').poshytip({
        className: 'tip-yellowsimple',
        showOn: 'focus',
        alignTo: 'target',
        alignX: 'center',
        offsetX: 0,
        offsetY: 5
    });
    $("#ValidCode").keydown(function (e) {
        var key = e.which;
        if (key == 13) {
            this.blur();
            SubAdd();
        }
    })
    $("input[name=findType]").click(function () {
        var val = $(this).val();
        if (val == 2) {
            $("#Mobile").val("");
            $("#tr_email").hide();
            $("#tr_mobile").show();
        } else {
            $("#Email").val("");
            $("#tr_email").show();
            $("#tr_mobile").hide();
        }
    });
    $("input[name=findType]").eq(0).click();
})
//成功
function AjaxSuccess(content) {
    hideMessage(0);
    if (content.IsSuccess) {
        if (content.PageTitle != null && content.PageTitle != "") {
            if ($("#table").find("#RegAnswer").length <= 0) {
                $(content.PageTitle).appendTo($("#table"));
                $("<input type='hidden' name='findType' value='" + $("input[name=findType]:checked").val() + "' />").appendTo($("#imgcode").parent());
                $("<input type='hidden' name='UserName' value='" + $("#UserName").val() + "' />").appendTo($("#imgcode").parent());
                $("<input type='hidden' name='Email' value='" + $("#Email").val() + "' />").appendTo($("#imgcode").parent());
                $("<input type='hidden' name='Mobile' value='" + $("#Mobile").val() + "' />").appendTo($("#imgcode").parent());
                $("<input type='hidden' name='ValidCode' value='" + $("#ValidCode").val() + "' />").appendTo($("#imgcode").parent());
                $("#r1,#r2,#UserName,#Email,#Mobile,#ValidCode").attr({ "name": " ", "disabled": "disabled" });
                $("#tr_name,#tr_email,#tr_mobile,#tr_vcode").hide();
                $("#imgcode").hide();
            } else {
                $("input[name=ValidCode][type=hidden]").remove();
                $("#tr_vcode").show();
                $("#ValidCode").attr({ "name": "ValidCode", "disabled": false });
                $("#imgcode").show();
                $("#imgcode").click();
                showMessage("<span class='box-error'>提示</span>", "密码提示答案错误!", true, false, true);
                hideMessage(1);
            }
        } else {
            DG.curWin.showMessage("<span class='box-success'>提示</span>", content.TipMsg, true, false, true);
            DG.cancel();
        }
    }
    else {
        $("#imgcode").click();
        showMessage("<span class='box-error'>提示</span>", content.TipMsg, true, false, true);
    }
}