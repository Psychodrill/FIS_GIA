var $        = require('jquery');
var routes   = require('../../Config/staffRoutes.js');
var uiAddons = require('../../Utils/ui-addons.js');

var checkRegion = function (row) {
	$.post(routes.checkRegion(row.data().Id))	
		.done(function (resp) {
	        resp.Region = resp.Region || row.data().Region;
			row.data(resp).draw();
		})
		.fail(function (xhr) {
			if (xhr.status === 404) {
				uiAddons.showMessage('Hет данных о регионе');
			} else {
				uiAddons.showMessage('Ошибка сервера');
			}
		});
};

module.exports = checkRegion;