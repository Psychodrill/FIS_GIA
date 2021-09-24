(function () {
	
	var $     = require('jquery');
	var login = require('./login.js');

	$('form').on('submit', login);

})();