var $ = require('jquery'),
    _ = require('underscore'),
    statuses = require('../../Config/appeal-statuses.json'),
    date = require('../../Utils/date.js'),
    ui = require('./../../Utils/ui.js');


var buildTable = function(resp) {
    var table = ui.buildTable({
        header: [{
                label: 'Дата',
                renderer: function(rowData) {
                    return date.dateTimeToRuDateFormat(rowData.Date);
                }
            }, {
                label: 'Статус апелляции',
                renderer: function(rowData) {
                    var status = _.findWhere(statuses, { Status: rowData.Status });
                    if (status) {
                        return status.Description;
                    }
                    return '';
                }
            }],
        data: resp.Appeals
    }).table;

    $('#table-container').html(table);

};

module.exports = buildTable;