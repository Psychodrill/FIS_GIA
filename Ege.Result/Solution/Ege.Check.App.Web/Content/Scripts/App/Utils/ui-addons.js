var $ = require('jquery');

var showMessage = function(msg, success) {
    var className = 'msg-popup';
    if (success) {
        className += ' green';
    }
    var div = $('<div>', {
        'class': className,
    });
    div.append(msg);

    $('body').append(div.click(function(e) {
        e.stopPropagation();
    }));
};

var closeMessages = function() {
    $('.msg-popup').remove();
};

$(document)
    .on('click', closeMessages)
    .on('scroll', closeMessages);

module.exports = {
    showMessage: showMessage
};