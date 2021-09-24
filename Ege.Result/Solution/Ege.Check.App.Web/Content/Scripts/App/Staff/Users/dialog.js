var $ = require('jquery'),
    _ = require('underscore'),
    dialog = require('../../Utils/dialog.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var save = function(el, dialogObj, table, postUrl) {
    var data = el.find('form').serializeArray(),
        passwordInput = el.find('#pass');

    if (_.findWhere(data, { name: 'Login' }).value.length === 0 ||
        passwordInput.val().length === 0) {
        return;
    }

    if (passwordInput.val() != passwordInput.get(0).defaultValue) {
        data.push({
            name: 'Password',
            value: passwordInput.val()
        });
    }

    $.post(postUrl, data)
        .done(function() {
            dialogObj.close();
            uiAddons.showMessage('Пользователь успешно сохранён', true);
            table.ajax.reload();
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось сохранить данные');
        });
};

var open = function(table, title, postUrl, data) {
    var dlg = dialog({
        title: title,
        contentTemplate: '#dlg-tmpl',
        contentData: data,
        actions: [{
                name: 'Сохранить',
                primary: true,
                onClick: function(el) {
                    save(el, dlg, table, postUrl);
                }
            }, {
                name: 'Отменить',
                close: true
            }],
        onSubmit: function(el) {
            save(el, dlg, table, postUrl);
        }
    });
};


module.exports = open;