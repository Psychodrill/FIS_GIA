var $ = require('jquery'),
    routes = require('./../../Config/routes.js'),
    hasLocalStorage = require('./../../Utils/local-storage.js');

var logout = function(e) {
    e.preventDefault();

    $.post(routes.logout)
        .done(function() {
            if (hasLocalStorage) {
                localStorage.removeItem('username');
            }
            location.href = routes.startPage;
        });
};

module.exports = logout;