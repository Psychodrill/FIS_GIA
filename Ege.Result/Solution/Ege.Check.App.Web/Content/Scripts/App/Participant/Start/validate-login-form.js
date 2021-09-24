var _ = require('underscore'),
    $ = require('jquery'),
    ui = require('./../../Utils/ui.js');

var validateCode = function(code) {
    if (code.replace(/[^0-9]+/g, '').length != 12) {
        return false;
    }
    return true;
};

var validate = function(fields, data) {

    var result = true;

    var highlight = function(id) {
        $('#' + id).addClass('error');
    };

    $('.error').removeClass('error');

    _.each(fields, function(field) {
        if (_.isString(field) && !data.hasOwnProperty(field) && field !== 'patr') {
            result = false;
            highlight(field);
        }
        if (_.isArray(field)) {
            var count = _.reduce(field, function(memo, curr) {
                return data.hasOwnProperty(curr) ? memo + 1 : memo;
            }, 0);
            if (count === 0) {
                result = false;
                _.each(field, function(f) {
                    highlight(f);
                });
            }
        }
    });

    if (!result) {
        ui.showEntryFormMessages.notValid(true);
    }

    if (data.hasOwnProperty('regNum') && !validateCode(data.regNum)) {
        ui.showEntryFormMessages.notValidCode(result);
        result = false;
        highlight('regNum');
    }

    if ($("#agreeNum")[0].checked != true) {
        ui.showEntryFormMessages.notAgreeCode(result);
        result = false;
    }

    return result;
};

module.exports = validate;