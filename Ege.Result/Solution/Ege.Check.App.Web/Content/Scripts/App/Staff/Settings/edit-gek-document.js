var $ = require('jquery');
require('jquery-ui');
require('../../Utils/datepicker-ru.js');

var //$ = require('jquery'),
    isFct = require('./../is-fct.js'),
    dialog = require('../../Utils/dialog.js'),
    uiAddons = require('../../Utils/ui-addons.js'),
    date = require('../../Utils/date.js'),
    routes = require('../../Config/staffRoutes.js');
//var datepickerRu = require('../../Utils/datepicker-ru.js');

var getValue = function(sel) {
    var el = $(sel);
    return el.val() ? el.val().trim() : null;
};

var openDialog = function(examId, cell, onSave, getRegion) {
    var dlg;

    var save = function(dialogEl) {

        dialogEl.find('form').off('submit').on('submit', function(e) {
            e.preventDefault();

            var data = {
                Number: getValue('#gek-doc-number'),
                CreateDate: getValue('#gek-doc-date'),
                Url: getValue('#gek-doc-link')
            };

            var url = isFct() ? routes.editFctGekDocument(examId, getRegion())
                : routes.editGekDocument(examId);

            $.post(url, data)
                .done(function() {
                    dlg.close();
                    uiAddons.showMessage('Данные успешно сохранены', true);
                    onSave(true);
                })
                .fail(function() {
                    dlg.close();
                    uiAddons.showMessage('Ошибка сервера. Не удалось сохранить данные');
                });

        }).find('button').trigger('click');

    };

    var actions = [
        {
            name: 'Сохранить',
            primary: true,
            onClick: save
        },
        {
            name: 'Отменить',
            close: true
        }
    ];

    if (cell.data()[0]) {
        actions.splice(1, 0, {
            name: 'Удалить',
            onClick: function(dialogEl) {
                var url = isFct() ? routes.deleteFctGekDocument(examId, getRegion())
                    : routes.deleteGekDocument(examId);
                $.ajax({
                    type: 'delete',
                    url: url,
                    success: function(resp) {
                        dlg.close();
                        onSave(false);
                        uiAddons.showMessage('Протокол ГЭК удалён', true);
                    },
                    error: function() {
                        uiAddons.showMessage('Ошибка сервера');
                    }
                });
            }
        });
    }

    dlg = dialog({
        title: 'Информация о протоколе ГЭК',
        contentTemplate: '#gek-dialog',
        sourceUrl: isFct() ? routes.getFctGekDocument(examId, getRegion()) : routes.getGekDocument(examId),
        parseData: function(resp) {
            if (resp && resp.CreateDate) {
                resp.CreateDate = date.dateToRuFormat(resp.CreateDate);
            }
            if (resp && resp.ExamDate) {
                resp.ExamDate = date.dateToRuFormat(resp.ExamDate);
            }
            return resp;
        },
        actions: actions,
        onSubmit: save,
        open: function(dialogEl) {
            $('#gek-doc-date').datepicker();
        },
        close: function() {
            $('#gek-doc-date').datepicker('destroy');
        }
    });
};

module.exports = openDialog;