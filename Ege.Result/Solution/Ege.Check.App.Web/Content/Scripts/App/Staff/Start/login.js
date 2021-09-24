var $ = require('jquery'),
    _ = require('underscore'),
    ui = require('./../../Utils/ui.js'),
    routes = require('./../../Config/staffRoutes.js');

var fields = ['Login', 'Password'],
    button = $('#submit');

var getFormData = function() {
    var valid = true;
    return {
        data: _.reduce(fields, function(memo, curr) {
            var value = $('#' + curr.toLowerCase()).val();
            if (value && value.length && value.trim().length) {
                memo[curr] = value.trim();
            } else {
                valid = false;
            }
            return memo;
        }, {}),
        valid: valid
    };
};

var login = function(e) {
    var fd = getFormData();

    if (!fd.valid) {
        ui.showEntryFormMessages.notValid(true);
        return;
    }

    var preloader = ui.showBtnPreloader(button);
    $.post(routes.login, fd.data)
        .done(function() {
            location.href = routes.settingsPage;
        })
        .fail(function() {
            ui.hideBtnPreloader(button, preloader);
            ui.showEntryFormMessages.authFailed(true);
        });
};

module.exports = login;