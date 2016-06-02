//定义预加载图片列表的函数(有参数)  
jQuery.preloadImages = function () {
    //遍历图片
    for (var i = 0; i < arguments.length; i++) {
        jQuery("<img>").attr("src", arguments[i]);
    }
}
// 你可以这样使用预加载函数
//$.preloadImages("images/logo.png", "images/logo-face.png", "images/mission.png"); 