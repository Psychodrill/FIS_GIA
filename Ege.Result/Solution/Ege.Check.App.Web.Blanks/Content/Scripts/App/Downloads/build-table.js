var $          = require('jquery');
//var dataTable  = require('datatables');
var dataTable = require('datatables')(window, $);
var dtLanguage = require('../Utils/dataTable-ru.js');
var date       = require('../Utils/date.js');
var routes     = require('../Config/routes.js');

var tablePreloader;
var tableContainer = $('#table-container');

var dateColRenderer = function(d) {
    return date.dateTimeToRuFormat(d);
};

var downloadColRenderer = function (data, type, full, meta) {
	if (data) {
		return '<a href="' + routes.donwloadZip(full.Id) + '" class="icon-download" title="Скачать архив"></a>';
	}
	return '';
};

var build = function () {
	var table = $('table').DataTable({
		ajax: function(data, callback, settings) {
            $.ajax({
                url: routes.getDownloads,
                data: {
                    take: data.length,
                    skip: data.start
                },
                success: function(json) {
                    var o = {
                        recordsTotal: json.Count,
                        recordsFiltered: json.Count,
                        data: json.Page
                    };
                    callback(o);
                    stopLoading();
                }
            });
        },
        columns: [
            { data: 'CreateDate', render: dateColRenderer, className: 'width-80' },
            { data: 'State', className: 'width-80' },
            { data: 'Total'},
            { data: 'Downloaded'},
            { data: 'Error'},
            { data: 'TotalParticipants'},
            { data: 'SuccessfullyProcessedParticipants'},
            { data: 'ProcessedWithErrorsParticipants'},
            { data: 'NotFoundParticipants'},
            { data: 'Note', className: 'width-120' },
            { data: 'IsReady', render: downloadColRenderer, className: 'width-40' }
        ],
        searching: false,
        autoWidth: false,
        info: false,
        paging: true,
        serverSide: true,
        ordering: false,
        language: dtLanguage,
        lengthMenu: [10, 20]
	});

	var stopLoading = function() {
        clearTimeout(tablePreloader);
        tableContainer.removeClass('loading');
    };

    table.on('preXhr', function() {
        tablePreloader = setTimeout(function() {
            tableContainer.addClass('loading');
        }, 200);
    });
};

module.exports = build;