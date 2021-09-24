var $ = require('jquery'),
	_ = require('underscore'),
	setSubmitHandler = require('./set-submit-handler.js'),
	captcha = require('./captcha.js'),
	mask = require('mask'),
	chosen = require('chosen'),
	keyboard = require('../../Config/keyboard.js');

var regnumInput = $('#regNum'),
	passnumInput = $('#passNum'),
	regNumClone = $('#regNumClone'),
	regionSelect = $('#region'),
	regnumFormat = '9999-9999-9999',
	regnumEmptyValue = '____-____-____';

var init = function (token) {
	setSubmitHandler(token);
	
	$('#reset-captcha').on('click', _.throttle(captcha.reset, 1000));
	
	$(document).on('captcha:reset', function (e, param) {
		setSubmitHandler(param);
	});

	var toggleDisable = function (target, input, condition) {
		input.prop('disabled', condition(target.val()));
	};

	regnumInput
		.on('keyup', function (e) {
			if (e.keyCode == keyboard.TAB) {
				return;
			}
			toggleDisable(regnumInput, passnumInput, function (value) {
				return value != regnumEmptyValue;
			});
		})
		.on('keydown', function (e) {
			if (keyboard.isNumber(e.keyCode)) {
				regNumClone.empty();
			}
		})
		.on('blur', function () {
			if (regnumInput.val() == regnumEmptyValue) {
				regnumInput.val('');	
			}
			toggleDisable(regnumInput, passnumInput, function (value) {
				return value.length;
			});	
			regNumClone.empty();
		})
		.on('focus', function () {
			if (!regnumInput.val().length) {
				regNumClone.text(regnumEmptyValue);
			}
		});

	passnumInput.on('keyup', function () {
		toggleDisable(passnumInput, regnumInput, function (value) {
			return value.length;
		});
	});

	regnumInput.mask(regnumFormat, {
		autoclear: false
	});

	regionSelect.chosen({
		placeholder_text_single: '&nbsp;',
		no_results_text: 'нет результатов'
	});
};

module.exports = init;