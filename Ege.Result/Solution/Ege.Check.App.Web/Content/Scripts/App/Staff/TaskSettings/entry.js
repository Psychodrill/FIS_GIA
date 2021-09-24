(function() {

    var $ = require('jquery'),
        initLogout = require('../logout.js'),
        fillSubjects = require('./fill-subjects.js'),
        buildTables = require('./build-tables.js');

    $('#task-settings-link').addClass('active');
    initLogout();

    var subjectsSelect = $('#subjects');
    fillSubjects(subjectsSelect);

    subjectsSelect.on('change', buildTables);

})();