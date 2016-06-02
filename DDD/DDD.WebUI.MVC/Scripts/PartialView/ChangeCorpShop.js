$(function () {
    //定义可使用/不可使用/是否所有门店可用的jquery变量
    var $canUse = $("#selCanUseShops"), $canNotUse = $("#selCanNotUseShops");
    var $all = $("#AllShops"), $all1 = $("#AllShops1"), $CanUseVal = $("#CanUseShops"), $CanNotUseVal = $("#CanNotUseShops");
    var isall = $("#txtIsAllShop").val();
    //控制显示可使用/不可使用门店
    if (isall == "True") {
        $("#CanUseShopFlag").hide();
        $("#CannotUseShopFlag").hide();
    } else {
        if ($CanUseVal.val() != "") {
            $("#CanUseShopFlag").show();
            $("#CannotUseShopFlag").hide();
        } else {
            $("#CanUseShopFlag").hide();
            $("#CannotUseShopFlag").show();
        }
    }
    //声明单选按钮的单击事件
    $("input[name=AllShopsFlag]").change(function () {
        if ($(this).val() == "True") {
            $("#CanUseShopFlag").hide();
            $("#CannotUseShopFlag").hide();
            $("#hidCanUseShop").val("");
            //所有门店可用移除全部
            $canUse.find("li").each(function () {
                RemoveItem($(this), $all, $all1, $CanUseVal, $CanNotUseVal, true);
            });
            $canNotUse.find("li").each(function () {
                RemoveItem($(this), $all1, $all, $CanNotUseVal, $CanUseVal, false);
            });
            //设置存放可使用不可使用门店ids的input标签值
            SetValue($canUse, $CanUseVal, $CanNotUseVal, false);
        } else {
            if ($(this).attr("id") == "r2") {
                $("#CanUseShopFlag").show();
                $("#CannotUseShopFlag").hide();
                //可使用门店:移除不可使用选项
                $canNotUse.find("li").each(function () {
                    RemoveItem($(this), $all1, $all, $CanNotUseVal, $CanUseVal, true);
                });
                //设置存放可使用不可使用门店ids的input标签值
                SetValue($canUse, $CanUseVal, $CanNotUseVal, true);
            } else if ($(this).attr("id") == "r3") {
                $("#CanUseShopFlag").hide();
                $("#CannotUseShopFlag").show();
                $("#hidCanUseShop").val("");
                //不可使用门店:移除可使用选项
                $canUse.find("li").each(function () {
                    RemoveItem($(this), $all, $all1, $CanUseVal, $CanNotUseVal, false);
                });
                //设置存放可使用不可使用门店ids的input标签值
                SetValue($canUse, $CanUseVal, $CanNotUseVal, false);
            }
        }
    });
    //判断如果加载时是所有门店可用:执行上面单击事件为所有门店可用
    if ($("input[name=AllShopsFlag]:checked").val() == "True") {
        $("#CanUseShopFlag").hide();
        $("#CannotUseShopFlag").hide();
        $canUse.find("li").each(function () {
            RemoveItem($(this), $all, $all1, $CanUseVal, $CanNotUseVal, true);
        });
        $canNotUse.find("li").each(function () {
            RemoveItem($(this), $all1, $all, $CanNotUseVal, $CanUseVal, false);
        });
        SetValue($canUse, $CanUseVal, $CanNotUseVal, false);
    } else {
        if ($CanUseVal.val() != "") {
            $("#CanUseShopFlag").show();
            $("#CannotUseShopFlag").hide();
            //可使用门店移除不可使用选项
            $canNotUse.find("li").each(function () {
                RemoveItem($(this), $all1, $all, $CanNotUseVal, $CanUseVal, true);
            });
        } else {
            $("#CanUseShopFlag").hide();
            $("#CannotUseShopFlag").show();
            //不可使用门店移除可使用选项
            $canUse.find("li").each(function () {
                RemoveItem($(this), $all, $all1, $CanUseVal, $CanNotUseVal, false);
            });
        }
    }
    //双击事件声明
    function bindClick() {
        //清除可使用门店&不可使用门店中的li的双击事件
        $("#AllShops >li,#AllShops1 >li").unbind('dblclick');
        //声明可使用门店的双击事件
        $("#AllShops >li").bind('dblclick', function () {
            //清除selection
            clearSelection();
            //如果向右移动 就移除当前项否则就填充当前项
            if ($(this).attr("path") == "right")
                RemoveItem($(this), $all, $all1, $CanUseVal, $CanNotUseVal, true);
            else
                FillItem($(this), $canUse, $all1, $CanUseVal, $CanNotUseVal, $canNotUse, true);
        });
        //声明不可使用门店的双击事件
        $("#AllShops1 >li").bind('dblclick', function () {
            //同上
            clearSelection();
            //同上
            if ($(this).attr("path") == "right")
                RemoveItem($(this), $all1, $all, $CanNotUseVal, $CanUseVal, false);
            else
                FillItem($(this), $canNotUse, $all, $CanNotUseVal, $CanUseVal, $canUse, false);
        });
        //清除可使用&不可使用门店的下拉菜单单击事件
        $("#selCanNotUseShops >li,#selCanUseShops >li,#AllShops >li,#AllShops1 >li").unbind('click');
        //声明单击事件
        $("#selCanNotUseShops >li,#selCanUseShops >li,#AllShops >li,#AllShops1 >li").bind('click', function () {
            //选中给它样式否则移除
            $(this).toggleClass('ui-selected');
        });
    }
    //操作渐变速度
    var speed = 200;
    //填充项
    function FillItem($item, $toObj, $clearObj, $valObj, $clearValObj, $clearObj1, isCanUseShopSet) {
        $clearObj.find("li[csid=" + $item.attr("csid") + "]").fadeOut(speed, function () { $(this).remove(); });
        $clearObj1.find("li").fadeOut(speed, function () { });
        if ($clearObj1.find("li").length > 0) {
            $clearObj1.clone(false).find("li").attr("path", "left").appendTo($all1).fadeIn();
            $clearObj1.find("li").attr("path", "left").appendTo($all).fadeIn();
            bindClick();
        }
        if ($item.attr("csid") == "")
            $item.remove();
        $item.attr("path", "right").fadeOut(speed, function () {
            $item.removeClass("ui-selected").appendTo($toObj).fadeIn(speed, function () {
                SetValue($toObj, $valObj, $clearValObj, isCanUseShopSet);
            });
        });
    }
    //移除项
    function RemoveItem($item, $toObj, $clearObj, $valObj, $clearValObj, isCanUseShopSet) {
        $item.attr("path", "left");
        var $parent = $item.parent();
        var clone = $item.clone(false);
        $item.fadeOut(speed, function () {
            clone.removeClass("ui-selected").appendTo($clearObj);
            $item.removeClass("ui-selected").appendTo($toObj).fadeIn(speed, function () {
                SetValue($parent, $valObj, $clearValObj, isCanUseShopSet);
                bindClick();
            });
        });
    }
    //填充所有项
    function FillAll($formObj, $toObj, $clearObj, $valObj, $clearValObj, $clearObj1, isCanUseShopSet) {
        $formObj.find("li").each(function () {
            if ($(this).hasClass("ui-selected"))
                FillItem($(this), $toObj, $clearObj, $valObj, $clearValObj, $clearObj1, isCanUseShopSet);
        });
    }
    //移除所有项
    function RemoveAll($formObj, $toObj, $clearObj, $valObj, $clearValObj, isCanUseShopSet) {
        $formObj.find("li").each(function () {
            if ($(this).hasClass("ui-selected"))
                RemoveItem($(this), $toObj, $clearObj, $valObj, $clearValObj, isCanUseShopSet);
        });
    }
    //可使用门店按钮单击事件
    $('#fill-right-one').click(function () { FillAll($all, $canUse, $all1, $CanUseVal, $CanNotUseVal, $canNotUse, true); })//可使用门店右移选中
    $('#fill-left-one').click(function () { RemoveAll($canUse, $all, $all1, $CanUseVal, $CanNotUseVal, true); })//可使用门店左移选中
    $('#fill-right').click(function () { $all.find("li").addClass('ui-selected'); FillAll($all, $canUse, $all1, $CanUseVal, $CanNotUseVal, $canNotUse, true); })//可使用门店右移全部
    $('#fill-left').click(function () { $canUse.find("li").addClass('ui-selected'); RemoveAll($canUse, $all, $all1, $CanUseVal, $CanNotUseVal, true); })//可使用门店左移全部
    //不可使用门店按钮单击事件
    $('#fill-right-one-1').click(function () { FillAll($all1, $canNotUse, $all, $CanNotUseVal, $CanUseVal, $canUse, false); })//不可使用门店右移选中
    $('#fill-left-one-1').click(function () { RemoveAll($canNotUse, $all1, $all, $CanNotUseVal, $CanUseVal, false); })
    $('#fill-right-1').click(function () { $all1.find("li").addClass('ui-selected'); FillAll($all1, $canNotUse, $all, $CanNotUseVal, $CanUseVal, $canUse, false); })
    $('#fill-left-1').click(function () { $canNotUse.find("li").addClass('ui-selected'); RemoveAll($canNotUse, $all1, $all, $CanNotUseVal, $CanUseVal, false); })
    //清除下拉菜单中的所有选中selection
    var clearSelection = function () {
        if (document.selection && document.selection.empty) {
            document.selection.empty();
        } else if (window.getSelection) {
            var sel = window.getSelection();
            sel.removeAllRanges();
        }
    };
    //声明双击事件的方法
    bindClick();
    //声明加载默认门店方法
    function LoadShop() {
        var shopId = $CanUseVal.val();
        if (shopId.length > 0) {
            var temp = shopId.split(',');
            for (var i = 0; i < temp.length; i++) {
                $all.find("li").each(function () {
                    if ($(this).attr('csid') == temp[i]) {
                        FillItem($(this), $canUse, $all1, $CanUseVal, $CanNotUseVal, $canNotUse, true);
                    }
                });
            }
        } else {
            shopId = $CanNotUseVal.val();
            var temp = shopId.split(',');
            for (var i = 0; i < temp.length; i++) {
                $all1.find("li").each(function () {
                    if ($(this).attr('csid') == temp[i]) {
                        FillItem($(this), $canNotUse, $all, $CanNotUseVal, $CanUseVal, $canUse, false);
                    }
                });
            }
        }
    }
    //设置存放可使用不可使用门店ids的input标签值
    function SetValue($fromObj, $valObj, $clearValObj, isCanUseShopSet) {
        var v = "";
        var s = "";
        $fromObj.find("li").each(function () {
            v += $(this).attr('csid') + ",";
            s += $(this).text() + ",";
        });
        if (v.length > 0)
            v = v.substring(0, v.length - 1);
        if (isCanUseShopSet) {
            if (s.length > 0) {
                s = s.substring(0, s.length - 1);
            }
            $("#hidCanUseShop").val(s);
        } else {//不可使用门店和所有门店可用需要清空可使用门店名称
            $("#hidCanUseShop").val("");
        }
        $valObj.val(v);
        $clearValObj.val("");
    }
    //加载默认门店
    LoadShop();
});
//验证可使用门店/不可使用门店信息
function ShopValid() {
    //定义参数
    var $allFlag = $("input[name=AllShopsFlag]:checked"), $CanUseShops = $("#CanUseShops"), $CanNotUseShops = $("#CanNotUseShops");
    //类型(普通业务/营销活动)
    var type = $("#RangeType").val();
    var errormsg = "";
    switch (type) {
        case "1":
            //商品添加和修改 优惠券
            if ($allFlag.attr("id") == "r2") {
                errormsg = "请选择可使用的门店!";
            } else if ($allFlag.attr("id") == "r3") {
                errormsg = "请选择不可使用的门店!";
            }
            break;
        case "2":
            //营销活动
            if ($allFlag.attr("id") == "r2") {
                errormsg = "请选择可参与的门店!";
            } else if ($allFlag.attr("id") == "r3") {
                errormsg = "请选择不可参与的门店!";
            }
            break;
        default:
            break;
    }
    if ($allFlag.val() == "True") {
        return true;
    } else {
        if (($allFlag.attr("id") == "r2" && ($CanUseShops.val() == "" || $CanUseShops.val().length <= 0)) ||
            ($allFlag.attr("id") == "r3" && ($CanNotUseShops.val() == "" || $CanNotUseShops.val().length <= 0))) {
            showMessage2("<span class='box-error'>错误</span>", errormsg, true, false, false, 240);
            return false;
        } else {
            $("#selCanUseShops").removeClass("input-validation-error");
            $("#valid-CanUseShops").html("").removeClass("field-validation-error").hide();
        }
    }
    return true;
}