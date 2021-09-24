var $ = require('jquery'),
    regionsJSON = require('../../Config/regions.json'),
    routes = require('../../Config/staffRoutes.js');

var tableInitialized = false;

var fillregions = function(initTable) {
    var table;

    var select = $('<select>', {
        'id': 'regions'
    });

    select.append($('<option>', {
        'text': 'Выберите регион'
    }).prop('disabled', true).prop('selected', true));

    $.each(regionsJSON, function(i, item) {
        select.append($('<option>', {
            'text': item.RegionName,
            'value': item.REGION
        }));
    });

    $('#wave').before(select);

    select.on('change', function() {
        if (tableInitialized) {
            table.ajax.url(routes.getSettings(select.val()));
            table.ajax.reload();
        } else {
            tableInitialized = true;
            table = initTable();
        }
    });
};

module.exports = fillregions;