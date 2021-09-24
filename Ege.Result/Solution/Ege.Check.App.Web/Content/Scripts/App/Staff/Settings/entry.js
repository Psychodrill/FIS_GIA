(function() {

    var $ = jQuery = require('jquery'),
      initLogout = require('../logout.js'),
      buildTable = require('./build-table.js'),
      isFct = require('./../is-fct.js'),
      fillregions = require('./fill-regions.js');

  $('#settings-link').addClass('active');

  //  $(function() {
        initLogout();

        var initTable = buildTable();

        if (isFct()) {
            fillregions(initTable);
        }
    //});


})();