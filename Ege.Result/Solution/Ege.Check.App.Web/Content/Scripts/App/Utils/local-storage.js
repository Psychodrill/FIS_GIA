var testLS = function() {
    var test = 'test';
    try {
        localStorage.setItem(test, test);
        localStorage.removeItem(test);
        return true;
    } catch(e) {
        return false;
    }
};

var hasLocalStorage = function() {
    return testLS();
};

module.exports = hasLocalStorage;