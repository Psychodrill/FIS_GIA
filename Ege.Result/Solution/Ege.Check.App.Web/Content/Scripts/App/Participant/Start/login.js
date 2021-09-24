var _ = require('underscore'),
    $ = require('jquery'),
    md5 = require('md5'),
    utf8 = require('utf8'),
    transformFio = require('./transform-fio.js'),
    transformPassNum = require('./transform-pass-num.js'),
    validate = require('./validate-login-form.js'),
    //captcha = require('./captcha.js'),
    routes = require('./../../Config/routes.js'),
    ui = require('./../../Utils/ui.js'),
    hasLocalStorage = require('./../../Utils/local-storage.js');

var fields = ['surname', 'name', 'patr', ['passNum', 'regNum'], 'region', 'g-recaptcha-response', 'agreeNum'],
    button = $('#submit-btn');

var getFormData = function() {
    return _.reduce(_.flatten(fields), function(memo, curr) {
        var value = $('#' + curr).val();
        if (value && value.length && value.trim().length) {
            memo[curr] = value.trim();
        }
        return memo;
    }, {});
};

var getDataToSend = function(fd) {
    return {
        Hash: md5(utf8(transformFio.simplify(fd.surname, fd.name, fd.patr))),
        Code: fd.regNum ? fd.regNum.replace(/[^0-9]+/g, '') : null,
        Document: fd.passNum ? transformPassNum(fd.passNum) : null,
        Region: fd.region,
        AgereeCheck: fd.agreeNum
       // Captcha: fd.captcha
    };
};

var togglePreloader = function() {
    button.children().toggleClass('hidden');
    button.prop('disabled', !button.prop('disabled'));
};

var submitPseudoForm = function() {
    var iFrameWindow = document.getElementById("iframe").contentWindow;
    iFrameWindow.document.body.appendChild(document.getElementById("entry-form").cloneNode(true));
    var frameForm = iFrameWindow.document.getElementById("entry-form");
    frameForm.onsubmit = null;
    frameForm.submit();
};


var createtoken = function() {
    grecaptcha.reset();
};

var login = function(e) {
    var data = getFormData();

    if (!validate(fields, data)) {
        return false;
    }

    var preloader = ui.showBtnPreloader(button),
        dataToSend = getDataToSend(data);

    dataToSend.Token = e.data.token;
    dataToSend.reCaptureToken = data["g-recaptcha-response"];//data.g-recaptcha-response;

    $.post(routes.login, dataToSend)
        .done(function() {
            if (hasLocalStorage) {
                localStorage.setItem('username',
                    transformFio.getShortForm(data.surname, data.name, data.patr));
            }
            location.href = routes.examsPage;
        })
        .fail(function(err) {
            ui.hideBtnPreloader(button, preloader);
            createtoken();
            //captcha.reset();
            if (err.status === 500) {
                ui.showEntryFormMessages.internalError(true);
            } else {
                ui.showEntryFormMessages.withText(err.responseJSON, true);
            }
        });

    submitPseudoForm();
    return false;
};

module.exports = login;