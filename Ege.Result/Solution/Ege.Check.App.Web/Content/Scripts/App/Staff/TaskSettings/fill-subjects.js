var $ = require('jquery'),
    mustache = require('mustache'),
    routes = require('../../Config/staffRoutes.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var template = $('#subj-tmpl').text();

var fillSubjects = function(select) {
    $.get(routes.getSubjects)
        .done(function(resp) {
            select.append(mustache.render(template, {
                subjects: resp
            }));
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось загрузить предметы');
        });
};

module.exports = fillSubjects;