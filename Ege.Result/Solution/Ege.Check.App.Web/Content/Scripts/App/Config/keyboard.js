module.exports = {
    ENTER: 13,
    TAB: 9,
    isNumber: function(keyCode) {
        if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {
            return true;
        }
        return false;
    }
};