var $ = require('jquery'),
    _ = require('underscore'),
    routes = require('../../Config/staffRoutes.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var table;

var remove = function(e) {
    var row = $(e.target).parents('tr'),
        data = _.pick(table.row(row.get(0)).data(), 'Code', 'ExamGlobalId', 'RegionId');
    $.post(routes.uncancel, data)
        .done(function(resp) {
            if (resp) {
                row.remove();
            } else {
                uiAddons.showMessage('Не удалось удалить запись');
            }
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось удалить запись');
        });
};

var initRemoveBtn = function(tableObj) {
    table = tableObj;
    $('table').on('click', '.c-remove', remove);
};

module.exports = initRemoveBtn;