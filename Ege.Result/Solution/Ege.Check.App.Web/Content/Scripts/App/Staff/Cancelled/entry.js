(function() {

    var $ = require('jquery'),
        initLogout = require('../logout.js'),
        buildTable = require('./build-table.js'),
        initForm = require('./init-form.js'),
        submitForm = require('./submit-form.js'),
        initRemoveBtn = require('./init-remove-btn.js');

    $('#cancelled-link').addClass('active');

    initLogout();

    initForm();
    var table = buildTable();

    $('form').on('submit', function(e) {
        submitForm(e, table);
    });

    initRemoveBtn(table);

})();