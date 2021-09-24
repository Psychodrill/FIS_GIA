var $ = require('jquery'),
    _ = require('underscore'),
    dialog = require('./dialog.js'),
    regionsJSON = require('../../Config/regions.json'),
    routes = require('../../Config/staffRoutes.js');

var edit = function(e, table) {
    var row = $(e.target).parents('tr'),
        rowData = table.row(row.get(0)).data(),
        url = routes.editUser(rowData.Id),
        data = {
            regions: regionsJSON,
            user: rowData,
            roles: {
                '1': rowData.Role == 'Fct',
                '2': rowData.Role == 'Rcoi'
            }
        };

    var userRegion = _.findWhere(data.regions, { REGION: data.user.RegionId });
    if (userRegion) {
        userRegion.selected = true;
    }

    data.user.pass = '123456';

    dialog(table, 'Редактирование пользователя', url, data);
};

module.exports = edit;