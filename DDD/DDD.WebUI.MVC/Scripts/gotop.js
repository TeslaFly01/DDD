$(function () {
    var $backToTopTxt = "返回顶部", $backToTopEle = $('<div class="backToTop"></div>').appendTo($("body"))
        .attr("title", $backToTopTxt).click(function () {
            $("html, body").animate({ scrollTop: 0 }, 800);
        }), $backToTopFun = function () {
            var st = $(document).scrollTop(), winh = $(window).height();
            (st > 260) ? $backToTopEle.fadeIn() : $backToTopEle.fadeOut();
            //IE6下的定位
            if (!window.XMLHttpRequest) {
                $backToTopEle.css("top", st + winh - 166);
            }
        };
    $(window).bind("scroll", $backToTopFun);
    $backToTopFun();
});