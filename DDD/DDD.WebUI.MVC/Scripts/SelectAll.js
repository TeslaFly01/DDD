$(function () {
    /* 全选 */
    $("#selectAllItems").click(function () {
        $("input[name='checkitem']").prop('checked', this.checked);
    });
});