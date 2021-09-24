var indicate = function (promise, doFn, undoFn, delay) {
	delay = delay || 200;

	setTimeout(function () {
		if (promise.state() !== 'pending') {
			doFn();
			promise.then(undoFn);
		}
	}, delay);
};

module.exports = indicate;