var $        = require('jquery');
var ui       = require('../../Utils/ui.js');
var mustache = require('mustache');

var render = function (data, buttonTemplate) {
    var tableContainer = $('<div>');
	var tableObj = ui.buildTable({
	        name: 'Cочинение',
	        className: 'mb-10',
	        data: data,
	        header: [{
	                label: '№',
	                name: 'Number',
	                className: 'width-60'
	            }, {
	                label: 'Критерии',
	                name: 'Name',
	                cellClassName: 'input-cell c-criteria-name',
	                renderer: function (rowData, cellData) {
	                	return $('<input>', { type: 'text' }).val(cellData);
	                }
	            }]
	    });

    tableContainer.append(tableObj.table).append(mustache.render(buttonTemplate));

	tableContainer
        .off()
        .on('click', '.c-add-row', function() {
            tableObj.addRow({
                Type: 'A'
            });
        })
        .on('click', '.c-remove-row', function() {
            tableObj.removeLastRow();
        });
    
    return tableContainer;

};

module.exports = render;