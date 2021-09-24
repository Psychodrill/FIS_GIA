var $ = require('jquery'),
    _ = require('underscore'),
    mustache = require('mustache'),
    dialog = require('../../Utils/dialog.js');

var rowTemplate = $('#dlg-row-tmpl').text();

var getOutput = function(el, data) {
    var outputData = [];
    el.find('tbody tr').each(function() {
        var that = $(this);
        outputData.push({
            Number: that.find('.c-number').text() || null,
            Name: that.find('.c-name').val(),
            MaxValue: that.find('.c-maxvalue').val(),
            Type: data.Type
        });
    });
    return outputData;
};

var openDialog = function(data, rowEl, onSave) {

    var criteriaDialog = dialog({
        title: 'Критерии задания',
        contentTemplate: '#dlg-tmpl',
        contentData: data,
        className: 'dlg-criteria',
        actions: [{
                name: 'Сохранить',
                primary: true,
                onClick: function(el) {
                    onSave(getOutput(el, data));
                    criteriaDialog.close();
                }
            }, {
                name: 'Добавить строку',
                onClick: function(el) {
                    el.find('tbody').append(mustache.render(rowTemplate, {}));
                }
            }, {
                name: 'Удалить строку',
                onClick: function(el) {
                    el.find('tbody tr:last').remove();
                }
            }, {
                name: 'Закрыть',
                close: true
            }]
    });
};

module.exports = openDialog;