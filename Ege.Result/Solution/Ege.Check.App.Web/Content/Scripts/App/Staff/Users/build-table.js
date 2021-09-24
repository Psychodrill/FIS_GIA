var $ = require('jquery'),
    _ = require('underscore'),
    mustache = require('mustache'),
    //dataTable = require('datatables'),
    dataTable = require('datatables')(window, $),

    routes = require('../../Config/staffRoutes.js'),
    regionsJSON = require('../../Config/regions.json'),
    dtLanguage = require('../../Utils/dataTable-ru.js');

var regionsTemplate = $('#regions-tmpl').text(),
    actionsTemplate = $('#actions-cell-tmpl').text(),
    filterParams;

var clearFilter = function() {
    $('form input').val('');
    $('form select').find('option:first').prop('selected', true);
    filterParams = null;
};

var actionsCellRender = function() {
    return actionsTemplate;
};

var buildTable = function() {
    var table = $('table').DataTable({
        ajax: function(data, callback, settings) {
            var params = {};
            _.each(filterParams, function(item) {
                if (item.value.length) {
                    params[item.name] = item.value;
                }
            });
            params.take = data.length;
            params.skip = data.start;

            $.ajax({
                url: routes.getUsers,
                data: params,
                success: function(json) {
                    var o = {
                        recordsTotal: json.Count,
                        recordsFiltered: json.Count,
                        data: json.Users
                    };
                    callback(o);
                }
            });
        },
        columns: [
            { data: 'Login' },
            { data: 'RegionName' },
            { data: 'RoleName' },
            { render: actionsCellRender, className: 'no-padding width-80' }
        ],
        searching: false,
        autoWidth: false,
        info: false,
        paging: true,
        serverSide: true,
        ordering: false,
        language: dtLanguage
    });

    $('#regions').append(mustache.render(regionsTemplate, { regions: regionsJSON }));

    $('#filter').on('submit', function(e) {
        e.preventDefault();
        filterParams = $(this).serializeArray();
        table.ajax.reload();
    });

    $('#clear-filter').on('click', clearFilter);

    return table;
};

module.exports = buildTable;