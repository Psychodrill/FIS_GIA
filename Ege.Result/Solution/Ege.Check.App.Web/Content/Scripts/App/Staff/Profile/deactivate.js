var $ = require('jquery'),
    routes = require('../../Config/staffRoutes.js'),
    ui = require('../../Utils/ui-addons.js');

var deactivate = function() {
    $.post(routes.deactivate)
        .done(function() {
            location.href = routes.startPage;
        })
        .fail(function() {
            ui.showMessage('Ошибка сервера. Учётная запись не отключена');
        });
};

module.exports = deactivate;