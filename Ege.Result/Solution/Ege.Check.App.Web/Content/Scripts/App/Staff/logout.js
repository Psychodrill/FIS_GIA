var $ = require('jquery'),
    routes = require('./../Config/staffRoutes.js');

var logout = function(e) {
    e.preventDefault();

    $.post(routes.logout)
        .done(function() {
            location.href = routes.startPage;
        });
};

var initLogout = function() {
    $('#logout').on('click', function(e) {
        logout(e);
    });
};

module.exports = initLogout;