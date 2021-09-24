var $ = require('jquery'),
    routes = require('../../Config/staffRoutes.js'),
    ui = require('../../Utils/ui.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var form = $('form'),
    button = form.find('button');

var submit = function(e, table) {
    e.preventDefault();
    var data = form.serialize();

    form.find('input, select').prop('disabled', true);
    var preloader = ui.showBtnPreloader(button);

    $.post(routes.cancel, data)
        .done(function(resp) {
            if (resp) {
                uiAddons.showMessage('Результат экзамена отменён', true);
                table.ajax.reload();
            } else {
                uiAddons.showMessage('Результатов не найдено');
            }
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось добавить запись');
        })
        .always(function() {
            form.find('input, select').prop('disabled', false);
            ui.hideBtnPreloader(button, preloader);
        });
};

module.exports = submit;