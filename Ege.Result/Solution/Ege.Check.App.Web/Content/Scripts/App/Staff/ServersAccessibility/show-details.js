var $        = require('jquery');
var routes   = require('../../Config/staffRoutes.js');
var uiAddons = require('../../Utils/ui-addons.js');

var showDetails = function (id) {
	location.href = routes.getStatusDetailsForRegion(id);
};

module.exports = showDetails;