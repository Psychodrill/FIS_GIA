var $  = require('jquery');
var ui = require('../Utils/ui.js');
var routes = require('../Config/routes.js');

var login = function (e) {
	e.preventDefault();

	var form = $(e.target);
	var button = form.find('button');
	var data = form.serializeArray();

	if (!data[0].value.length || !data[1].value.length) {
        ui.showEntryFormMessages.notValid(true);
		return;
	}

    var preloader = ui.showBtnPreloader(button);
	
	$.post(routes.login, data)
		.done(function () {
			location.href = routes.blanksPage;
		})
		.fail(function () {
			ui.hideBtnPreloader(button, preloader);
            ui.showEntryFormMessages.authFailed(true);
		});
};

module.exports = login;