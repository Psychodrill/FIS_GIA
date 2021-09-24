var $ = require('jquery'),
    mustache = require('mustache'),
    initForm = require('./init-form'),
    routes = require('../../Config/staffRoutes.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var data,
    el = $('#docs'),
    template = $("#link-tmpl").text(),
    editTemplate = $('#edit-link-tmpl').text(),
    errMsg = 'Ошибка сервера. Не удалось загрузить документы';

var render = function(resp) {
    data = resp;
    el.html(mustache.render(template, data));
};

var edit = function() {
    el.html(mustache.render(editTemplate, data));
    initForm();
};

var load = function() {
    var timeout = setTimeout(function() {
        el.addClass('loading');
    }, 200);

    $.get(routes.documents)
        .done(render)
        .fail(function() {
            uiAddons.showMessage(errMsg);
        })
        .always(function() {
            clearTimeout(timeout);
            el.removeClass('loading');
        });

    $(document).on('click', '#edit-btn', edit);
};

module.exports = load;