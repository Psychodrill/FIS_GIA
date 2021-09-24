var $ = require('jquery'),
    _ = require('underscore'),
    mustache = require('mustache'),
    routes = require('../../Config/staffRoutes.js'),
    ui = require('../../Utils/ui.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var el = $('#docs'),
    addBtnSelector = '#add-row-btn',
    rowTemplate = $('#row-tmpl').text(),
    listTemplate = $('#link-tmpl').text(),
    successMsg = 'Данные успешно сохранены',
    errMsg = 'Ошибка сервера. Не удалось сохранить данные';

var addRow = function() {
    $('#docs-table tbody').append(mustache.render(rowTemplate, {}));
};

var removeRow = function(e) {
    var target = $(e.target);
    target.parents('tr').remove();
};

var save = function() {
    var data = {
        Documents: []
    };

    $('#docs-table tbody tr').each(function() {
        var row = $(this);
        data.Documents.push({
            Id: row.data('id'),
            Name: row.find('.c-name').val(),
            Url: row.find('.c-url').val()
        });
    });

    var button = $('#save'),
        preloader = ui.showBtnPreloader(button);

    $.post(routes.documents, data)
        .done(function() {
            uiAddons.showMessage(successMsg, true);
            el.html(mustache.render(listTemplate, data));
        })
        .fail(function() {
            uiAddons.showMessage(errMsg);
        })
        .always(function() {
            ui.hideBtnPreloader(button, preloader);
        });
};

var init = function() {
    $(document)
        .on('click', addBtnSelector, addRow)
        .on('click', '.c-remove-btn', removeRow)
        .on('click', '#save', save);
};

module.exports = init;