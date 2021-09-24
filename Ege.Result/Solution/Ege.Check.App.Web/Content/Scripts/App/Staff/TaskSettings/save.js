var $ = require('jquery'),
    _ = require('underscore'),
    routes = require('../../Config/staffRoutes.js'),
    ui = require('../../Utils/ui.js'),
    uiAddons = require('../../Utils/ui-addons.js');

var save = function(partB, otherParts, subjectId, isComposition) {

    var mapValues = function(container) {
        if (!container) {
            return;
        }

        var output = [];

        if (isComposition) {
            output.push({
                Type: "A",
                Number: 1,
                Criteria: []
            });

            container.find('tbody tr').each(function(i) {
                var row = $(this);
                if (!row.hasClass('empty')) {
                    output[0].Criteria.push({
                        Number: i + 1,
                        Type: 'A',
                        MaxValue: 1,
                        Name: row.find('.c-criteria-name input').val()
                    });
                }
            });
        } else {
            container.find('tbody tr').each(function() {
                var row = $(this),
                    rowData = row.data('row'),
                    legalSymbolsInput = row.find('.c-legal input'),
                    number = row.find('.c-number input').val(),
                    numValue = number && number.length ? number : 0;

                if (!row.hasClass('empty')) {
                    var data = {
                        MaxValue: row.find('.c-maxvalue input').val(),
                        Type: rowData.Type,
                        DisplayNumber: numValue
                    };

                    if (legalSymbolsInput.length) {
                        data.LegalSymbols = legalSymbolsInput.val();
                    }
                    if (rowData.Criteria) {
                        data.Criteria = _.omit(rowData.Criteria, 'Number');
                    }

                    output.push(data);
                }
            });
        }

        return output;
    };

    var data = {
        PartB: mapValues(partB),
        WithCriteria: mapValues(otherParts)
    };

    var button = $('#save'),
        preloader = ui.showBtnPreloader(button);

    $.post(routes.taskSettings(subjectId), data)
        .done(function() {
            uiAddons.showMessage('Данные успешно сохранены', true);
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось сохранить данные');
        })
        .always(function() {
            ui.hideBtnPreloader(button, preloader);
        });

};

module.exports = save;