var $        = require('jquery');
var _        = require('underscore');
var mustache = require('mustache');
var routes   = require('../Config/routes.js');
var ui       = require('../Utils/ui.js');

var form            = $('#multi-form');
var button          = $('#multi-download');
var notice          = form.find('.c-notice');
var filesList       = $('#files-list');
var fileInput       = $('#file-input');
var commentInput    = $('#comment');
var errorTemplate   = $('#error-tmpl').text();
var successTemplate = $('#multi-download-notice').text();
var fileTemplate    = $('#file-tmpl').text();
var filesArray      = [];

var submit = function (e) {

	notice.empty();
	var preloader = ui.showBtnPreloader(button);
	var fd = new FormData();
	_.each(filesArray, function (item, i) {
		fd.append(i, item);
	});
	fd.append(filesArray.length, commentInput.val());

	$.ajax({
	    url: routes.uploadCsv,
	    data: fd,
	    cache: false,
	    contentType: false,
	    processData: false,
	    type: 'POST',
	    success: function (resp) {
	    	notice.append(successTemplate);
	    	filesList.empty();
	    },
	    error: function () {
	    	notice.append(errorTemplate);
	    },
	    complete: function () {
	    	ui.hideBtnPreloader(button, preloader);
	    }
	});
};

var add = function (e) {
	$.each(e.target.files, function (i, file) {
		filesArray.push(file);
		filesList.append(mustache.render(fileTemplate, { name: file.name }));
	});
};

module.exports = {
	submit: submit,
	add: add
};