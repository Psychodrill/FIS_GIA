(function() {

    var $ = require('jquery'),
        initLogout = require('../logout.js'),
        loadDocuments = require('./load-documents.js');

    $('#docs-link').addClass('active');
    loadDocuments();
    initLogout();
})();