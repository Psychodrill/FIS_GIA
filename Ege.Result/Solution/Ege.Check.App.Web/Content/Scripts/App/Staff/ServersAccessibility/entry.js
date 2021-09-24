(function () {
	
	var $          = require('jquery');
	var buildTable = require('./build-table.js');
	var checkAll   = require('./check-all.js');
	var initLogout = require('../logout.js');

	$('#servers-link').addClass('active');
	initLogout();
	var table = buildTable();
	$('#check-all').click(function () {
		checkAll(table, $(this));
	});

})();