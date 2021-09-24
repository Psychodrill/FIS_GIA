(function () {

	var $              = require('jquery');
	var downloadSingle = require('./download-single.js');
	var uploadForm     = require('./upload-form.js');
	var ajaxSetup      = require('../Utils/ajax-setup.js');

	ajaxSetup();
	$('#single-download').on('submit', downloadSingle);
	$('#multi-download').on('click', uploadForm.submit);
	$('#file-input').on('change', uploadForm.add);

})();