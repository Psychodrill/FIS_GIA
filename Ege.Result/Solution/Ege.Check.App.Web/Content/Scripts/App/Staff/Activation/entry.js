(function() {

    var $ = require('jquery'),
        initLogout = require('../logout.js'),
        activate = require('./activate.js');

    initLogout();
    $('#activate').on('click', activate);

})();