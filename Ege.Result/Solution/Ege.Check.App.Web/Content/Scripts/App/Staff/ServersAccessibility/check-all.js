var $        = require('jquery');
var routes   = require('../../Config/staffRoutes.js');
var uiAddons = require('../../Utils/ui-addons.js');
var ui       = require('../../Utils/ui.js');

var checkAll = function (table, button) {
	var preloader = ui.showBtnPreloader(button);

	table.on('xhr', function () {
		ui.hideBtnPreloader(button, preloader);
	});

	table.ajax.url(routes.checkAllRegions).load();
};

module.exports = checkAll;