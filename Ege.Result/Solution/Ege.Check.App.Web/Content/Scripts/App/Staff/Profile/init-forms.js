var $ = require('jquery'),
    isFct = require('./../is-fct.js'),
    ui = require('../../Utils/ui.js'),
    uiAddons = require('../../Utils/ui-addons.js'),
    deactivate = require('./deactivate.js');

var options;

var successMsg = 'Данные успешно сохранены',
    errorMsg = 'Ошибка сервера. Не удалось сохранить данные',
    passwordNotValidMsg = 'Пароли не совпадают. Проверьте правильность введённых данных';

var save = function(form) {
    var button = form.find('button'),
        preloader = ui.showBtnPreloader(button),
        url = options[form.data('index')].dest,
        select = $('#regions-select');

    if (isFct() && select.length) {
        url += '/' + select.val();
    }

    $.post(url, form.serialize())
        .done(function() {
            uiAddons.showMessage(successMsg, true);
        })
        .fail(function() {
            uiAddons.showMessage(errorMsg);
        })
        .always(function() {
            ui.hideBtnPreloader(button, preloader);
        });
};

var valid = function(form) {
    var passInputs = form.find('input[type="password"]');
    if (passInputs.length === 0) {
        return true;
    }
    if (passInputs.eq(0).val() == passInputs.eq(1).val()) {
        return true;
    }
    uiAddons.showMessage(passwordNotValidMsg);
    return false;
};

var init = function(o) {
    options = o;

    $(document).on('submit', 'form', function(e) {
        e.preventDefault();
        var form = $(e.target);
        if (valid(form)) {
            save(form);
        }
    });

    $(document).on('click', '#deactivate', deactivate);
};

module.exports = init;