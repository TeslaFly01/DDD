/* File Created: 七月 26, 2012 */
$(function() {
    //getNewOrder();
    //getNewECoupon();
    var msgPanel = $(".right-box"),
        msgCtrl = $(".right-message .btn-home");
    msgCtrl.click(function() {
        msgPanel.hide();
    });
});

function openCorpLevel() {
    var CorpLevelDG =
            new J.dialog({
                id: 'divCorpLevel',
                title: '商家等级说明',
                page: '/CorpHelp/CorpHelp/Corplevel',
                width: 650,
                height: 470,
                rang: false,
                iconTitle: false,
                autoPos: true,
                resize: false,
                maxBtn: false,
                minBtn: false,
                xButton: true,
                cancelBtnTxt: '关闭'
            });
    CorpLevelDG.ShowDialog();
}

function getNewOrder() {
    //FusionCharts.setCurrentRenderer('javascript'); //强制采用js绘图，不用flash
    var chart1 = new FusionCharts({
        swfUrl: "/Content/FusionCharts/Column3D.swf",
        renderAt: "report1", width: "100%", height: "300"
    });
    chart1.configure("ChartNoDataText", "没有显示的数据");
    chart1.configure("ParsingDataText", "数据读取中,请稍后...");
    chart1.configure("LoadDataErrorText", "数据读取出现错误");
    chart1.configure("RenderingChartText", "数据读取中,请稍后...");
    chart1.configure("XMLLoadingText", "数据读取中,请稍后...");
    var url = encodeURI("/Reports/Report/NewOrderReport");
    chart1.setJSONUrl(url);
    chart1.render("newOrder");
}
function getNewECoupon() {
    var chart1 = new FusionCharts({
        swfUrl: "/Content/FusionCharts/Column3D.swf",
        renderAt: "report2", width: "100%", height: "300"
    });
    chart1.configure("ChartNoDataText", "没有显示的数据");
    chart1.configure("ParsingDataText", "数据读取中,请稍后...");
    chart1.configure("LoadDataErrorText", "数据读取出现错误");
    chart1.configure("RenderingChartText", "数据读取中,请稍后...");
    chart1.configure("XMLLoadingText", "数据读取中,请稍后...");
    var url = encodeURI("/Reports/Report/NewECouponReport");
    chart1.setJSONUrl(url);
    chart1.render("newEcoupon");
}
