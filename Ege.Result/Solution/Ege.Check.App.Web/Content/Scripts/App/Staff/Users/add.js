var dialog = require('./dialog.js'),
    regionsJSON = require('../../Config/regions.json'),
    routes = require('../../Config/staffRoutes.js');

var add = function(table) {
    dialog(table, 'Добавление пользователя', routes.createUser, {
        regions: regionsJSON
    });
};

module.exports = add;