var transform = function (num) {
	var legnth = 12;
	var count = num.toString().length;
	var prefix = new Array(legnth - count + 1).join('0');
	return prefix + num;
};

module.exports = transform;