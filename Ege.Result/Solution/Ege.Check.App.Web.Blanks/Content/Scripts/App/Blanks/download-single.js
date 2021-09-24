var $        = require('jquery');
var _        = require('underscore');
var routes   = require('../Config/routes.js');
var statuses = require('../Config/statuses.js');
var indicate = require('../Utils/indicate.js');
var ui       = require('../Utils/ui.js');

var errorClassName = 'error';
var noticeTemplate = $('#notice-tmpl').text();
var errorTemplate  = $('#error-tmpl').text();
var allowEmpty = ['Patronymic'];

var valid = function (form, data) {
	var result = true;
	form.find('.' + errorClassName).removeClass(errorClassName);

	_.each(data, function (item) {
		if (!item.value.length && !_.contains(allowEmpty, item.name)) {
			result = false;
			form.find('[name="' + item.name + '"]').addClass(errorClassName);
		}
	});

	return result;
};

var submit = function (e) {
	e.preventDefault();

	var form = $(e.target),
		data = form.serializeArray(),
		button = form.find('button');

	form.find('.c-notice').empty();

	if (valid(form, data)) {

		var preloader = ui.showBtnPreloader(button);

		$.post(routes.downloadArchiveByParticipant, data)
			.done(function (resp) {
				form.find('.c-notice').append($('<div>', {
					'class': 'notice mb-20 ' + (statuses[resp.Status].success ? 'info': ''),
					'text': statuses[resp.Status].text
				}));
				if (resp.Id && resp.Status === 'Success') {
					window.location.href = routes.donwloadZip(resp.Id);
				}
			})
			.fail(function () {
				form.find('.c-notice').append(errorTemplate);		
			})
			.always(function () {
				ui.hideBtnPreloader(button, preloader);
			});


	} else {
		form.find('.c-notice').append(noticeTemplate);
	}
};

module.exports = submit;
