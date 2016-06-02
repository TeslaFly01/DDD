/* File Created: 四月 1, 2013 */
//选择商品
function ChangeProduct() {
    var openWin =
    new J.dialog({
        id: 'divSearchCorp',
        title: '搜索商品',
        page: '/Product/Products/SearchCorpProduct/',
        width: 750,
        height: 470,
        rang: true, //限制挪动范围
        iconTitle: false, //图标
        autoPos: true, //自动定位  居中
        cover: true, //锁屏
        resize: false,
        maxBtn: false
    });
    openWin.ShowDialog();
}

//商品清空
function onempty() {
    $("#txtPrId").val("");
    $("#spSPName").html("");
    $("#imgshopname").hide();
    //判断是不是要显示查询条件
    if ($('#hidcgproduct').val() == "true") {
        $("#prod1").html("");
        $("#liprod").hide(1000);
    }
}