var $ = require('jquery'),
	routes = require('./../../Config/routes.js'),
	ui = require('./../../Utils/ui.js');

var captcha = {
	reset: function () {
		$.get(routes.captcha).done(function (resp) {
			ui.setCaptcha(resp.Image);
			$(document).trigger('captcha:reset', [resp.Token]);
			$('#captcha').val('');
		});	
	}
};

module.exports = captcha;