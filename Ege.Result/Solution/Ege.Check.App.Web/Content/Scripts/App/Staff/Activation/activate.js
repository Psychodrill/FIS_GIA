var $ = require('jquery'),
    routes = require('../../Config/staffRoutes.js'),
    ui = require('../../Utils/ui.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var activate = function(e) {
    var button = $(e.target),
        preloader = ui.showBtnPreloader(button);

    $.post(routes.activate)
        .done(function() {
            location.href = routes.settingsPage;
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Учётная запись не активирована');
        })
        .always(function() {
            ui.hideBtnPreloader(button, preloader);
        });
};

module.exports = activate;