var $ = require('jquery');

module.exports = function () {
	$.ajaxSetup({
		statusCode: {
			401: function () {
				location.href = "/";
			}
		}
	});
};	