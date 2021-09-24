/* 
 USAGE: jquery library
*/

function ajax() {

}

function showAsyncError(request, textStatus, errorThrown) {
	unblockUI();
	if (request.status == 500) {
		var iframe1 = jQuery('<iframe width="100%" height="100%" frameborder="no"></iframe>');
		var d = jQuery('<div></div>');
		iframe1.appendTo(d);
		d
			//.html('<iframe src="' + absoluteAppPath + 'Account/AjaxError" width="100%" height="100%" frameborder="no"></iframe>')
			.dialog({
				autoOpen: false,
				modal: true,
				width: 640,			height: 490,
				title: request.status + " - " + request.statusText
			}).dialog("open");
		iframe1.attr("src", absoluteAppPath + 'Account/AjaxError');
	} else if (request.status == 401) {
		var iframe1 = jQuery('<iframe width="100%" height="100%" frameborder="no"></iframe>');
		var d = jQuery('<div></div>');
		iframe1.appendTo(d);
		d
			//.html('<iframe src="' + absoluteAppPath + 'Account/AjaxError" width="100%" height="100%" frameborder="no"></iframe>')
			.dialog({
				autoOpen: false,
				modal: true,
				width: 640,	height: 490,
				title: request.status + " - " + request.statusText
			}).dialog("open");
		iframe1.attr("src", absoluteAppPath + 'Account/AuthError?statusID=4');
	} else {
		jQuery('<div></div>')
			.html(request.responseText)
			.dialog({
				autoOpen: false,
				modal: true,
				width: 640,
				height: 480,
				title: request.status + " - " + request.statusText
			}).dialog("open");
	}
}

function showAsyncFilePostError(request, textStatus, errorThrown) {
	unblockUI();
	if (request.responseText.indexOf('404.13') >= 0) //IIS BigRequest Error
		infoDialog('Вы пытаетесь загрузить слишком большой файл');
	else
		showAsyncError(request, textStatus, errorThrown);
}


function closeDialog($el) {
	$el.parent().children('div').children('a').children('span').first().click();
}

function confirmDialog(message, yesCallback, noCallback) { //////////////ДОБАВИЛ noCallback 
	jQuery('<div>' + message + '</div>').dialog(
		{
		    title: 'Подтверждение',
			resizable: false,
			modal: true,
			buttons: {
		        "Да": function() {
		            yesCallback();
		            jQuery(this).dialog("close");
		        },
		        "Нет": function() {
		            if (noCallback != undefined) {
		                noCallback();
		            }
		            jQuery(this).dialog("close");
		        }
		    }
		});
}

function infoDialog(message, closeCallback, closeDialogCallback) {
    jQuery('<div>' + message + '</div>').dialog({
        resizable: false,
        modal: true,
        buttons:{
            "Закрыть": function() {  if (closeCallback) closeCallback();  jQuery(this).dialog("close"); }
        },
        close: function () {         if (closeDialogCallback) { closeDialogCallback(); }    }
    });
}

function infoDialogC(message, param) {
    if (param == undefined) {  param = {}; }
    var p = {
        resizable: false, modal: true,
        //width: 640, height: 480,
        buttons: { "Закрыть": function () { if (param.closeCallback) { param.closeCallback(); } jQuery(this).dialog("close"); } },
        close: function () { if (param.closeDialogCallback) { param.closeDialogCallback(); } }
    };			
    if (param.width){   p.width=param.width;  }
    if (param.height) { p.width = param.height; }
    if (param.title) { p.title = param.title; }
    if (param.modal) { p.modal = param.modal; }

    jQuery('<div>' + message + '</div>').dialog(p);
}


/*var callbackWait = 300;
var callbackStart;

// показываем блокировку минимум 300 мс, чтобы избежать моргания

function doPostAjax(navigateUrl, postData, successCallback)
{
	jQuery.ajax({
		contentType: 'application/json; charset=utf-8',
		type: 'POST',
		dataType: 'json',
		data: postData,
		url: navigateUrl,
		success: function (data, status)
		{
			var callbackFinish = new Date();
			if (callbackFinish - callbackStart < callbackWait)
				setTimeout(function ()
				{
					jQuery.unblockUI();
					successCallback(data, status);
				}, callbackWait - (callbackFinish - callbackStart));
			else
			{
				jQuery.unblockUI();
				successCallback(data, status);				
			}
		},
		error: showAsyncError
	});

	callbackStart = new Date();

	jQuery.blockUI(
	{
		fadeIn: 0,
		fadeOut: 0,
		css: { opacity: .7 },
		baseZ: 1003,
		message: "<div class='busy'><a href='javascript:void(0)' class='busy' onclick='return false'></a>Пожалуйста подождите...</div>"
	});
}*/


var callbackWait = 600;
var callbackStart;
var callbackReturn = true;

// показываем прозрачный gif на странице до 600 мс, если сообщение не пришло,
// то показываем фон и сообщение "подождите", ждем ответа от callback и скрываем блокировку.

// в качестве postContentType можно использовать "application/x-www-form-urlencoded" для запросов сервера с параметром

function doPostAjax(navigateUrl, postData, successCallback, postContentType, resultDataType, doUIBlock) {
	jQuery.ajax({
		contentType: (postContentType != null ? postContentType : 'application/json; charset=utf-8'),
		type: 'POST',
		dataType: (resultDataType != null ? resultDataType : 'json'),
		data: postData,
		url: navigateUrl,
		success: function(data, status) {
			if (doUIBlock == null || doUIBlock) unblockUI();
			successCallback(data, status);
		},
		error: showAsyncError
	});
	if (doUIBlock == null || doUIBlock) blockUI();
}

function doPostAjaxSync(navigateUrl, postData, successCallback, postContentType, resultDataType, doUIBlock) {

    try {
        if (doUIBlock == null || doUIBlock) blockUI();
        jQuery.ajax({
            contentType: (postContentType != null ? postContentType : 'application/json; charset=utf-8'),
            type: 'POST',
            dataType: (resultDataType != null ? resultDataType : 'json'),
            data: postData,
            url: navigateUrl,
            async: false,
            success: function (data, status) {
                if (doUIBlock == null || doUIBlock) unblockUI();  
                successCallback(data, status);
            },
            error: showAsyncError
        });
    } catch (e){
    } finally{
      if (doUIBlock == null || doUIBlock) unblockUI();    
    }
}

function doGetAjax(navigateUrl, postData, successCallback, postContentType, resultDataType, doUIBlock) {
    jQuery.ajax({
        contentType: (postContentType != null ? postContentType : 'application/json; charset=utf-8'),
        type: 'GET',
        dataType: (resultDataType != null ? resultDataType : 'json'),
        data: postData,
        url: navigateUrl,
        success: function (data, status) {
            if (doUIBlock == null || doUIBlock) unblockUI();
            successCallback(data, status);
        },
        error: showAsyncError
    });
    if (doUIBlock == null || doUIBlock) blockUI();
}


function blockUI() {
	callbackStart = new Date();
	callbackReturn = false;

	jQuery.blockUI({ fadeIn: 0,	fadeOut: 0,	baseZ: 1003, message: null,	overlayCSS: {	opacity: 0.0}});

	setTimeout(function() {
		if (!callbackReturn) {
			jQuery.unblockUI();
			jQuery.blockUI({	fadeIn: 200,	fadeOut: 200,	baseZ: 1004,	overlayCSS: {	opacity: 0.7	},
				message: "<div class='busy'><a href='javascript:void(0)' class='busy' onclick='return false'></a>Пожалуйста, подождите...</div>"
			});
		}
	}, callbackWait);
}

function unblockUI() {
	callbackReturn = true;
	jQuery.unblockUI();
}


var isValidationErrorBase = false;

function clearValidationErrors($parent) {
	isValidationErrorBase = false;
	$parent.find('input.input-validation-error').addClass('input-validation-error-fixed').removeClass('input-validation-error');
	$parent.find('select.input-validation-error').addClass('input-validation-error-fixed').removeClass('input-validation-error');
	$parent.find('.field-validation-error').remove().detach();
}

function addValidationError($control, message, newLine) {
	if ($control.attr('val-new-line') != undefined)
		newLine = $control.attr('val-new-line') == '1';
	isValidationErrorBase = true;
	$control.addClass('input-validation-error').removeClass('input-validation-error-fixed');
	if (message) {
		if ($control.hasClass('datePicker') || (typeof $control.attr('autocomplete') != "undefined")) $control = $control.next();
		$control.after(' <span class="field-validation-error">'
			+ (newLine || newLine == null ? '<br/>' : '')
			+ message + '</span>');
	}
}

function revalidatePage($parent, newLine) {
	clearValidationErrors($parent);
	$parent.find('input[val-required]').each(function () {
	    if ($(this).val() == '') {
	        addValidationError($(this), $(this).attr('val-required'), newLine);
	    }
	});
	return isValidationErrorBase;
}

function addValidationErrorsFromServerResponse(data, newLine) {
	if (data.IsError) {
		if (data.Data == null || data.Data.length == 0)
			alert(data.Message);
		else {
			var minTop = jQuery('html').position().top;
			for (var i = 0; i < data.Data.length; i++) {
				$ctrl = jQuery('#' + data.Data[i].ControlID);
				if ($ctrl != null && $ctrl.length > 0) {
					var top = $ctrl.position().top;
					if (top < minTop) minTop = top;
				}
				addValidationError($ctrl, data.Data[i].ErrorMessage, newLine);
			}
			if (minTop != jQuery('html').position().top)
				jQuery('html').animate({ scrollTop: minTop - 2 }, 400);
		}
		return true;
	}
	return false;
}

function escapeHtml(text) {
	if (text == null) return null;
	return text.replace( /&/g , '&amp;').replace( /</g , '&lt;').replace( /</g , '&gt;');
}

function unescapeHtml(text) {
	if (text == null) return null;
	return text.replace( /&amp;/g , '&').replace( /&lt;/g , '<').replace( /&gt;/g , '>');
}

function isFileLengthCorrect(inputFile, maxLength) {
	if (inputFile.files != null && inputFile.files.length > 0) {
		if (inputFile.files[0].size != undefined) {
			return inputFile.files[0].size <= maxLength;
		}
	}
	return true;
}

function fillPager(totalPageCount, currentPage)
{
	var pageWindowCount = 10;
	if (totalPageCount < 2) {
		jQuery('#divPager,#divPagerSep').hide();
		return;
	}
	var res = '';
	if (currentPage > 0) {
		res += '<a href="#" onclick="movePager(' + 0 + ');return false;" class="pageLink pageLinkArrowLeftLeft">&nbsp;</a>';
		res += '<a href="#" onclick="movePager(' + (currentPage - 1) + ');return false;" class="pageLink pageLinkArrowLeft">&nbsp;</a>';
	} else {
		res += '<a href="#" onclick="return false;" class="pageLink pageLinkArrowLeftLeft">&nbsp;</a>';
		res += '<a href="#" onclick="return false;" class="pageLink pageLinkArrowLeft">&nbsp;</a>';
	}

	var minShownPage = Math.floor(currentPage / pageWindowCount) * pageWindowCount;
	var maxShownPage = Math.min(minShownPage + pageWindowCount, totalPageCount);

	if (minShownPage > 0) {
		res += '<a href="#" onclick="movePager(' + (minShownPage - pageWindowCount) + ');return false;" class="pageLink">...</a>';
	}
	for (var i = minShownPage; i < maxShownPage; i++) {
		if (i != currentPage)
		{
			if(currentPage - pageWindowCount < i && currentPage + pageWindowCount > i)
				res += '<a href="#" onclick="movePager(' + (i) + ');return false;" class="pageLink">' + (i + 1) + '</a>';
		} else
			res += '<a href="#" onclick="return false;" class="pageLink pageLinkActive">' + (i + 1) + '</a>';
		//if (i + 1 < totalPageCount)
		//	res += ' ... ';
	}
	if (maxShownPage < totalPageCount)
	{
		res += '<a href="#" onclick="movePager(' + (maxShownPage) + ');return false;" class="pageLink">...</a>';
	}
	if (currentPage + 1 < totalPageCount) {
		res += '<a href="#" onclick="movePager(' + (currentPage + 1) + ');return false;" class="pageLink pageLinkArrowRight">&nbsp;</a>';
		res += '<a href="#" onclick="movePager(' + (totalPageCount - 1) + ');return false;" class="pageLink pageLinkArrowRightRight">&nbsp;</a>';
	} else {
		res += '<a href="#" onclick="return false;" class="pageLink pageLinkArrowRight">&nbsp;</a>';
		res += '<a href="#" onclick="return false;" class="pageLink pageLinkArrowRightRight">&nbsp;</a>';
	}
	jQuery('#divPager').html(res);
	jQuery('#divPager,#divPagerSep').show();
}

function autocompleteDropdown($ctrl, options) {
	options.open = function() { jQuery(this).attr('opened', '1'); };
	options.close = function() {
		var $el = jQuery(this);
		$el.attr('opened', '0');
		$el.autocomplete('option', 'minLength', $el.attr('minLength'));
	};
	var minLength = 3;
	if (typeof options.minLength != 'undefined')
		minLength = options.minLength;
	$ctrl.autocomplete(options);
	$ctrl.attr('minLength', minLength);
	if ($ctrl.attr('autocomplete') != '1') {
		$ctrl.attr('autocomplete', '1');
		$ctrl.after('<img title="" class="ui-datepicker-trigger gvuz-calendar-icon" alt="..." src="' + absoluteAppPath + 'Resources/Images/ddl.png"/>');
		$ctrl.next().click(function() {
			var $el = jQuery(this).prev();
			if ($el.attr('disabled')) return;
			if (!$el.attr('opened') || $el.attr('opened') == '0') {
				$el.autocomplete('option', 'minLength', 0);
				$el.autocomplete('search', '');
			} else {
				$el.autocomplete('close');
			}
		});
	}
}

function ModeSwitcher() {
	this.editBtnSelector = '.editButton';
	this.saveBtnSelector = '.saveButton';
	this.cancelBtnSelector = '.cancelButton';
	this.targetSelector = '.editable';
	for (var n in arguments[0]) {
		this[n] = arguments[0][n];
	}

	var editContainerClassName = "switcherEditContainer";
	var isReadOnly = true;
	this.getEditContainerSelector = function() {
		return '.' + editContainerClassName;
	};
	this.setEditMode = function() {
		if (isReadOnly) {
			jQuery(this.editBtnSelector).hide();
			jQuery(this.saveBtnSelector).show();
			jQuery(this.cancelBtnSelector).show();
			jQuery('.linkSumulator').attr('disabled', true); // disable sorting
			jQuery('.pageBlock').hide(); // disable paging

			makeEditable(this.targetSelector);
			isReadOnly = false;
		}
	};
	this.setReadOnlyMode = function(isCancelled) {
		if (!isReadOnly) {
			jQuery(this.editBtnSelector).show();
			jQuery(this.saveBtnSelector).hide();
			jQuery(this.cancelBtnSelector).hide();
			jQuery('.linkSumulator').attr('disabled', false); // enable sorting
			jQuery('.pageBlock').show(); // enable paging


			if (isCancelled)
				makeReadOnlyByCancelClicked(this.targetSelector);
			else
				makeReadOnlyBySaveClicked(this.targetSelector);
			isReadOnly = true;
		}
	}; // if cancel button clicked then restore old values using this array
	var values = [];
	var makeEditable = function(selector) {
		values.length = 0;
		jQuery.each(jQuery(selector), function(index, object) {
			var value = jQuery(object).text();
			values.push(value);
			jQuery(object).html('<input type="text" value="' + value + '" class="' + editContainerClassName + '"/>');
		});
	};
	var makeReadOnlyByCancelClicked = function(selector) {
		jQuery.each(jQuery(selector), function(index, object) {
			jQuery(object).html(values[index]);
		});
	};
	var makeReadOnlyBySaveClicked = function(selector) {
		jQuery.each(jQuery(selector), function(index, object) {
			var value = jQuery(object).find('.' + editContainerClassName).val();
			jQuery(object).empty();
			jQuery(object).append(value);
		});
	};
}

function getCookie(c_name) {
	var i, x, y, ARRcookies = document.cookie.split(";");
	for (i = 0; i < ARRcookies.length; i++) {
		x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
		y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
		x = x.replace( /^\s+|\s+$/g , "");
		if (x == c_name) {
			return unescape(y);
		}
	}
}

function setCookie(c_name, value, exdays) {
	var exdate = new Date();
	exdate.setDate(exdate.getDate() + exdays);
	var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
	document.cookie = c_name + "=" + c_value;
}

/*IE7 array prototype enchance */
if(!Array.indexOf) 
	Array.prototype.indexOf = function(v) { for(var i = 0; i < this.length; i++) {if(v == this[i]) return i;};return -1;};

jQuery.fn.setDisabled = function ()
{
	this.attr('disabled', 'disabled');
	return this;
}
jQuery.fn.setEnabled = function ()
{
	this.removeAttr('disabled');
	return this;
}

jQuery.fn.toggleDisabled = function (isDisabled)
{
	if (isDisabled) this.attr('disabled', 'disabled');
	else this.removeAttr('disabled');
	return this;
}
jQuery.fn.toggleEnabled = function (isEnabled)
{
	if (!isEnabled) this.attr('disabled', 'disabled');
	else this.removeAttr('disabled');
	return this;
}

jQuery.fn.attrToArr = function (attrName)
{
	var arr = []
	this.each(function (i, e)
	{
		arr.push(jQuery(e).attr(attrName))
	})
	return arr;
}