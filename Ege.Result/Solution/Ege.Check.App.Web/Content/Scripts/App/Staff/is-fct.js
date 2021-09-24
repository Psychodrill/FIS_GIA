var $ = require('jquery');

var input = $('#is-fct');

var isFct = function() {
    return input.length && input.val() === 'True';
};

module.exports = isFct;