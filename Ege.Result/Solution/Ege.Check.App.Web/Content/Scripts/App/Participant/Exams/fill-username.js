var $ = require('jquery'),
    hasLocalStorage = require('./../../Utils/local-storage.js');

var fillUsername = function() {
    if (hasLocalStorage) {
        $('#username').text(localStorage.getItem('username'));
    }
};

module.exports = fillUsername;