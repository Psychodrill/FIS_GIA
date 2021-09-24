var $ = require('jquery'),
    //dataTable = require('datatables'),
    dataTable = require('datatables')(window, $),
    routes = require('../../Config/staffRoutes.js'),
    dtLanguage = require('../../Utils/dataTable-ru.js'),
    date = require('../../Utils/date.js');

var table,
    tablePreloader,
    tableEl = $('#table'),
    tableContainer = $('#table-container');

var dateColRenderer = function(d) {
    return date.dateToRuFormat(d);
};

var removeColRenderer = function() {
    return '<button class="btn-small btn-light c-remove">Удалить</button>';
};

var build = function() {
    table = tableEl.DataTable({
        ajax: function(data, callback, settings) {
            $.ajax({
                url: routes.getCancelled,
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
            { data: 'RegionName' },
            { data: 'Code' },
            { data: 'Date', render: dateColRenderer },
            { data: 'SubjectName' },
            { render: removeColRenderer, className: 'no-padding' }
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

    return table;
};

module.exports = build;