/* File Created: 五月 4, 2012 */
var LEFT_MENU_VIEW = 1;

function leftmenu_ctrl() {
    var myarrow = document.getElementById("myarrow");

    if (LEFT_MENU_VIEW == 0) {
        parent.document.getElementById("frm2").cols = "178,8,*";
        LEFT_MENU_VIEW = 1;
        myarrow.src = '/Content/images/close.jpg';
    }
    else {
        parent.document.getElementById("frm2").cols = "0,8,*";
        LEFT_MENU_VIEW = 0;
        myarrow.src = '/Content/images/open.jpg';
    }
}