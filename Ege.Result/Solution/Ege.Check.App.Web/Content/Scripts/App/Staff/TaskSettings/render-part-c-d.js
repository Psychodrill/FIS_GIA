var $ = require('jquery'),
    _ = require('underscore'),
    mustache = require('mustache'),
    dialog = require('./dialog.js'),
    ui = require('../../Utils/ui.js');

var headers = {
    'C': 'Ответы на задания с развёрнутым ответом',
    'D': 'Ответы на задания устной части'
};

var inputCellRenderer = function(rowData, cellData) {
    var value = cellData === 0 ? '' : cellData;
    return $('<input>', { type: 'text' }).val(value);
};

var render = function(data, buttonTemplate, type) {
    var tableContainer = $('<div>');
    var tableObj = ui.buildTable({
            name: headers[type],
            className: 'mb-10',
            data: data,
            header: [{
                    label: '№',
                    name: 'DisplayNumber',
                    className: 'width-60',
                    cellClassName: 'input-cell c-number',
                    renderer: inputCellRenderer
                }, {
                    label: 'Максимальный балл',
                    name: 'MaxValue',
                    cellClassName: 'input-cell c-maxvalue',
                    renderer: inputCellRenderer
                }, {
                    label: 'Критерии',
                    cellClassName: 'c-criteria-cell',
                    renderer: function(rowData) {
                        var text = 'нет';
                        if (rowData.Criteria && rowData.Criteria.length) {
                            text = rowData.Criteria.length;
                        }

                        return $('<a>', {
                            'href': '#',
                            'text': text
                        });
                    }
                }]
            });

    tableContainer.append(tableObj.table).append(mustache.render(buttonTemplate));

    tableContainer
        .off()
        .on('click', '.c-add-row', function() {
            tableObj.addRow({
                Type: type
            });
        })
        .on('click', '.c-remove-row', function() {
            tableObj.removeLastRow();
        })
        .on('click', 'a', function(e) {
            e.preventDefault();
            var target = $(e.target),
                row = target.parents('tr'),
                rowData = row.data('row');

            dialog(rowData, row, function(taskCriteria) {
                rowData.Criteria = taskCriteria;
                row.data('row', rowData);
                target.text(taskCriteria.length || 'нет');
            });
        });
    return tableContainer;
};

module.exports = render;