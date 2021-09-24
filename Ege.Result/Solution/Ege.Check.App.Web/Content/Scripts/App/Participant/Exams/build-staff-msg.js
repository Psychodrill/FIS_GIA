var $ = require('jquery');

var container = $('#rcoi-msg');

var build = function (info) {
	if (info.Info) {
		var message = $('<div>', { 'class': 'notice warning mb-20' });

		if (info.HotlinePhone) {
			message.append($('<div>', {
				'class': 'notice-tag',
				'text': 'Телефон горячей линии РЦОИ: ' + info.HotlinePhone
			}));
		}

		message
			.append($('<h5>', { 'text': 'Информация от РЦОИ' }))
			.append($('<div>', { 'text': info.Info }));

		container.append(message);
	}
};

module.exports = build;