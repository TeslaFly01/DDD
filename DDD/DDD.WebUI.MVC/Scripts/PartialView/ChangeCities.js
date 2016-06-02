/* File Created: 一月 17, 2014 */
$(function () {
    //城市
    $("#CityID").dblclick(function () { fillSelectedCity('CityID', 'selectBox', 'CityIDs'); });
    var cities = $("#txtCityIDs").val();
    GetCities($("#txtProvinceID").val(), $("#txtCityID").val(), (cities == ""));
    if (cities != "" && cities != undefined) {
        loadCity(cities, 'CityID', 'selectBox', 'CityIDs');
    }
});

//填充城市
function fillSelectedCity(id1, id2, id3) {
    $("#" + id1).find(":selected").each(function () {
        $("<li class='pli' tem='" + $(this).val() + "'><span>" + $(this).text() + "</span><a href='#' class='del-city' onclick='delCity(\"" + id1 + "\",\"" + id2 + "\",\"" + id3 + "\",this)' ></a></li>").appendTo($("#" + id2).find("ul"));
        $(this).remove();
    });
    $("#" + id3).val("");
    var temp = "";
    $("#" + id2).find(".pli").each(function () {
        temp += $(this).attr("tem") + ",";
    });
    if (temp.length > 1)
        temp = temp.substring(0, temp.length - 1);
    $("#" + id3).val(temp);
    $("#" + id1)[0].selectedIndex = 0;
}

//删除城市
function delCity(id1, id2, id3, v) {
    var oldObj = $(v).parents(".pli");
    var obj = $("<option value='" + oldObj.attr('tem') + "'>" + oldObj.text() + "</option>");
    var flag = false;
    $("#" + id1).find(":selected").each(function () {
        if ($(this).val() == oldObj.attr('tem'))
            flag = true;
    });
    if (!flag)
        oldObj.fadeOut(1, function () {
            oldObj.remove();
            $(obj).appendTo($("#" + id1));
            $("#" + id1)[0].selectedIndex = -1;
            fillSelectedCity(id1, id2, id3);
        });
}

//ajax获取城市
function GetCities(pid, cid, isFill) {
    if (pid == null || pid == "") {
        showMessage("<span class='box-error'>错误</span>", "请选择一个有效的省份", true, false, false);
        return false;
    }
    $.ajax(
        {
            url: "/Region/City/GetCities/?pid=" + pid,
            type: "get",
            dataType: "json",
            success: function (result) {
                $("#CityID").html("");
                if (result == undefined || result == null) {
                    showMessage("<span class='box-error'>错误</span>", "网络出现错误,请稍后重试...", true, false, true);
                    return false;
                }

                var old = $("#txtCityIDs").val().split(",");
                for (var i = 0; i < result.length; i++) {
                    if (result[i]["Value"] == undefined || result[i]["Value"] == "" || result[i]["Text"] == undefined || result[i]["Text"] == "")
                        continue;
                    if (SetCity(old, result[i]))
                        $("<option value='" + result[i]["Value"] + "'>" + result[i]["Text"] + "</option>").appendTo($("#CityID"));
                    else
                        continue;
                }
                $("#CityID").val(cid);
                if (isFill)
                    fillSelectedCity('CityID', 'selectBox', 'CityIDs');
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                showMessage("<span class='box-error'>错误</span>", errorThrown, true, false, true);
            }
        });
}

//设置城市值
function SetCity(old, item) {
    if (old.length > 0) {
        for (var k = 0; k < old.length; k++) {
            if (item["Value"] == old[k]) {
                return false;
            }
        }
    }
    return true;
}

//加载城市
function loadCity(ids, id1, id2, id3) {
    $.ajax({
        url: "/Region/City/GetCitiesByIds/?ids=" + ids,
        cache: false,
        type: "get",
        dataType: "json",
        success: function (data) {
            var item = data.split(',');
            var arrids = ids.split(',');
            for (var i = 0; i < item.length; i++) {
                if (i >= arrids.length || item[i] == "")
                    continue;
                $("<li class='pli' tem='" + arrids[i] + "'>" + item[i] + " &nbsp;<img style='cursor:pointer' src='/content/images/xx.gif' onclick='delCity(\"" + id1 + "\",\"" + id2 + "\",\"" + id3 + "\",this)' > </li>").appendTo($("#" + id2).find("ul"));
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络错误，请稍后再试!");
        }
    });
}
