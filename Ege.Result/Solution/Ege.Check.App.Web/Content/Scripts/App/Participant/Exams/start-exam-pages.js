var $ = require('jquery'),
	routes = require('./../../Config/routes.js'),
	cookieHelper = require('./../../Utils/cookie.js'),
	logout = require('./logout.js'),
	fillUsername = require('./fill-username.js');

var start = function () {
    if (cookieHelper.getCookie('Participant')) {
		$('body').removeClass('empty');
	} else {
		location.href = routes.startPage;
	}

	fillUsername();
	$('#logout').click(logout);
};

module.exports = start;