var $ = require('jquery');

var maxLength = 14,
	strMaxLength = 12,
	delimiter = '-';

var isLetter = function (keyCode) {
	return keyCode >= 65 && keyCode <= 90;
};

var isNumber = function (keyCode) {
	return (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105);
};

var mask = function (input) {
	
	input
		.on('keydown', function (e) {
	        if (isLetter(e.which)) {
	            return false;
	        }
	        if (isNumber(e.which) && input.val().length >= maxLength) {
	        	return false;
	        }
	        return true;
		})
		.on('keyup', function (e) {
			if (isNumber(e.which)) {
	        	input[0].value = input[0].value + '+';
	        }
		})
		.on('keypress', function () {
		
		});

};	

module.exports = mask;