var $ = require('jquery'),
    routes = require('../../Config/staffRoutes.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var remove = function(e, table) {
    if (confirm('Вы уверены, что хотите удалить пользователя?')) {

        var row = $(e.target).parents('tr'),
            data = table.row(row.get(0)).data();

        $.ajax({
            type: 'delete',
            url: routes.deleteUser(data.Id),
            success: function() {
                table.row(row.get(0)).remove().draw();
            },
            error: function() {
                uiAddons.showMessage('Ошибка сервера. Не удалось удалить пользователя');
            }
        });
    }
};

module.exports = remove;