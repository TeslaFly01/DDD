$.validator.addMethod("twosqrt", function (value, element) {
    if (value.length == 0) {
        return true;
    }
    if (value == 0) return false;
    else {
        if (((value - 1) & value) == 0) return true;
        else return false;
    }
});
$.validator.unobtrusive.adapters.addBool("twosqrt");