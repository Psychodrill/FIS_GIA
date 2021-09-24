(function () {
	
	var buildTable = require('./build-table.js');
	var ajaxSetup  = require('../Utils/ajax-setup.js');

	ajaxSetup();
	buildTable();

})();