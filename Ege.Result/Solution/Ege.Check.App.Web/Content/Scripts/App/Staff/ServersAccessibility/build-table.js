var $           = require('jquery');
//var dataTable   = require('datatables');
var dataTable = require('datatables')(window, $);
var showDetails = require('./show-details.js');
var checkRegion = require('./check-region.js');
var dtLanguage  = require('../../Utils/dataTable-ru.js');
var date        = require('../../Utils/date.js');
var routes      = require('../../Config/staffRoutes.js');

var tableEl = $('#table');

var statusColRender = function (d) {
	return $('#' + (d ? 'check-icon' : 'block-icon')).text();
};

var btnColRender = function () {
	return $('#table-buttons').text();	
};

var dateCellRender = function (d) {
	return d ? date.dateTimeToRuFormat(d) : '';
};

var buildTable = function () {
	var table = tableEl.DataTable({
        ajax: {
            url: routes.getAvailabilityStatus,
            dataSrc: function(data) {
                return data;
            }
        },
		columns: [
			{ data: 'IsAvailable', render: statusColRender, className: 'no-padding' },
			{ data: 'Region', className: 'align-left' },
			{ data: 'Server', className: 'align-left break-all' },
			{ data: 'ServerCount' },
			{ data: 'DbCount' },
			{ data: 'LastFileCheck', render: dateCellRender },
			{ render: btnColRender, className: 'no-padding btn-col' }
		],
		paging: false,
        searching: false,
        autoWidth: false,
        info: false,
        ordering: false,
        language: dtLanguage
	});

	tableEl
		.on('click', '.c-more', function (e) {
			showDetails(table.row($(e.target).parents('tr').get(0)).data().Id);
		})
		.on('click', '.c-check', function (e) {
			checkRegion(table.row($(e.target).parents('tr').get(0)));
		});

	return table;
};

module.exports = buildTable;