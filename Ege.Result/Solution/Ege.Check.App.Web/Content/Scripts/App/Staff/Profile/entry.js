(function() {

    var $ = require('jquery'),
        initLogout = require('../logout.js'),
        isFct = require('./../is-fct.js'),
        initTabs = require('./init-tabs.js'),
        routes = require('../../Config/staffRoutes.js'),
        initForms = require('./init-forms.js');

    $('#profile-link').addClass('active');

    initLogout();

    var tabOptions = [
        {
            source: routes.regionInfo,
            dest: routes.regionInfo,
            chooseRegion: isFct()
        },
        {
            dest: routes.setPassword
        }
    ];

    var tabs = initTabs(tabOptions);
    tabs.setTab(0);

    initForms(tabOptions);

})();