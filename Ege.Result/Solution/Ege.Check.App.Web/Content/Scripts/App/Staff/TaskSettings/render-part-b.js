var $ = require('jquery'),
    mustache = require('mustache'),
    ui = require('../../Utils/ui.js');

var inputCellRenderer = function(rowData, cellData) {
    return $('<input>', { type: 'text' }).val(cellData);
};

var render = function(data, buttonTemplate) {
    var tableObj = ui.buildTable({
        name: 'Ответы на задания с кратким ответом',
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
                className: 'width-180',
                cellClassName: 'input-cell c-maxvalue',
                renderer: inputCellRenderer
            }, {
                label: 'Допустимые символы',
                name: 'LegalSymbols',
                cellClassName: 'input-cell c-legal',
                renderer: inputCellRenderer
            }]
    });

    var container = $('<div>')
        .append(tableObj.table)
        .append(mustache.render(buttonTemplate));

    container
        .on('click', '.c-add-row', function() {
            tableObj.addRow({
                Type: 'B'
            });
        })
        .on('click', '.c-remove-row', function() {
            tableObj.removeLastRow();
        });

    return container;
};

module.exports = render;