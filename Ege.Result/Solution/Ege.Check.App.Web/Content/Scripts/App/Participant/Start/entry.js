(function() {

    var $ = require('jquery'),
        routes = require('./../../Config/routes.js'),
        fillRegions = require('./fill-regions'),
        initForm = require('./init-form.js'),
        cookieHelper = require('./../../Utils/cookie.js'),
        ui = require('./../../Utils/ui.js');
    
    if (cookieHelper.getCookie('Participant')) {
        location.href = routes.examsPage;
    }

    $.when($.get(routes.captcha), $.get(routes.region))
        .done(function(captchaResp, regionResp) {
            ui.setCaptcha(captchaResp[0].Image);
            initForm(captchaResp[0].Token);
            fillRegions(regionResp[0]);
            $('body').removeClass('empty');
        })
        .fail(function() {
            $('body').removeClass('empty');
            ui.showError($('#content'));
        });

})();