/* File Created: 五月 4, 2012 */
$.ajaxSetup({ cache: false });
$(function () {
    scrollHeight();
    $(window).resize(scrollHeight);
    $('body').rollbar({ zIndex: 100 });
    $.ajax({
        type: "get",
        url: '/Home/LeftMenu',
        dataType: "html",
        //beforeSend: function () {
        //   $("#load-s").show();
        //},
        error: function (XMLHttpRequest, textStatus, errorThrown) { alert("加载功能菜单失败，请稍后再试！"); /*alert(XMLHttpRequest.responseText);*/ },
        complete: function () {
            $("#load-menu").hide();
        },
        success: function (data) {
            $("#menu-main").html(data);
            $(".menu-title").click(function () {
                $(this).next("div").animate({ height: "toggle" }, "fast").siblings(".menu-body").slideUp("fast");
                $(this).siblings(".menu-title").children(".menu-title-up").addClass("menu-title-down").removeClass("menu-title-up");
                $(this).children("span").toggleClass("menu-title-up").toggleClass("menu-title-down");
            });
            $(".ll").click(function () {
                $(".ll").removeClass("cur");
                $(this).addClass("cur");
            });
        }
    });

});

function scrollHeight() {
    var winHeight = $(window).height();
        $('body').css("height", winHeight);
}