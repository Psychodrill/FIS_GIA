(function() {

    var $ = require('jquery'),
        initLogout = require('../logout.js'),
        buildTable = require('./build-table.js'),
        remove = require('./remove.js'),
        add = require('./add.js'),
        edit = require('./edit.js');

    $('#users-link').addClass('active');
    initLogout();

    var table = buildTable();

    $('table')
        .on('click', '.c-remove', function(e) {
            remove(e, table);
        })
        .on('click', '.c-edit', function(e) {
            edit(e, table);
        });

    $('#add').on('click', function() {
        add(table);
    });

})();