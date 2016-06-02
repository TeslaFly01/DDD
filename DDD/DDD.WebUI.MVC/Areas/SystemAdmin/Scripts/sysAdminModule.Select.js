$(document).ready(function () {
    //module下拉列表选择
    $("#selectSearch").change(function () {
        var parFid = $(this).val();
        if (parFid == "") {
            alert("您的选择有误!");
            return;
        }
        $.ajax({
            url: "Index/?Fid=" + parFid, type: "get", beforeSend: function () { showMessage("", "正在请求，请等待！", false, true, true); },
            dataType: "json",
            success: function (result) {
                hideMessage(1);
                if (result.IsSuccess) {
                    $("#divModuleList").load(result.ReDirectUrl);
                }
                else {
                    showMessage("<span class='box-error'>失败</span>", result.TipMsg, true, false, false);
                }
            },
            error: function () { hideMessage(0); showMessage("<span class='box-error'>错误</span>", "网络错误,请稍后再试!", true, false, false); }
        });
    });
});