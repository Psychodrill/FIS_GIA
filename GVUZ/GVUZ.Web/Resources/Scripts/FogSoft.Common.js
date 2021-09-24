(function($) {

	// FogSoft.Common
	$.extend(true, window, { FogSoft: { Common: function() {} }});

	FogSoft.Common.formatBoolean = function(value, pattern) {
		var tokens = (pattern || "").split("|");

		if (value === true) return tokens.length > 0 ? tokens[0] : "Да";
		if (value === false) return tokens.length > 1 ? tokens[1] : "Нет";
		if (value === null) return tokens.length > 2 ? tokens[2] : "";
		return value;
	};

	FogSoft.Common.formatNumber = function(value, pattern, dataType) {
		if (value != null) {
			if (pattern) {
			}
			else if (dataType == "number") {
				pattern = "#,##0";
			}
			else if (dataType == "money") {
				pattern = "#,##0.00";
			}
			return $.formatNumber(value, { format: pattern || "", locale: "ru" });
		}
		return value;
	};

	FogSoft.Common.formatDateTime = function(value, pattern, dataType) {
		if (value != null) {
			var d = new Date(parseInt(value.replace( /\/Date\(-?(\d+)\)\//gi , "$1")));
			if (pattern) {
			}
			else if (dataType == "date") {
				pattern = "dd.MM.yyyy";
			}
			else if (dataType == "datetime") {
				pattern = "dd.MM.yyyy HH:mm";
			}
			else if (dataType == "time") {
				pattern = "HH:mm";
			}
			return $.format.date(d, pattern);
		}
		return value;
	};

	FogSoft.Common.showSuccessMessage = function(message) {
		$('#ajax-status-message').html(message).removeClass().addClass("ajax-success-message").fadeIn().delay(1000).fadeOut();
	};
	
	FogSoft.Common.showWarningMessage = function(message) {
		$('#ajax-status-message').html(message).removeClass().addClass("ajax-warning-message").fadeIn()
			.click(function () {
				$(this).fadeOut();
			});
	};

	FogSoft.Common.showErrorMessage = function(message) {
		$('#ajax-status-message').html(message).removeClass().addClass("ajax-error-message").fadeIn()
			.click(function () {
				$(this).fadeOut();
			});
	};
	
	FogSoft.Common.refreshPage = function() {
		var href = window.location.href;
		if (href.lastIndexOf('#') == href.length)
			href = href.substr(0, href.lenght - 1);
		window.location = href;
	};	
})(jQuery);