var $ = require('jquery'),
	submitClickHandler = require('./login.js');

var setSubmitHandler = function (token) {
	$('#entry-form').off().on('submit', { token: token }, submitClickHandler);
};

module.exports = setSubmitHandler;