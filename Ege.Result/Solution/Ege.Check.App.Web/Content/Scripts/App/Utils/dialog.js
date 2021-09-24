var $ = require('jquery'),
    mustache = require('mustache'),
    uiAddons = require('./ui-addons.js'),
    keyboard = require('../Config/keyboard.js');

var makeEl = function(className) {
    return $('<div>', {
        'class': className
    });
};

var dialog = function(o) {

    var overlay = makeEl('overlay'),
        dialog = makeEl('dialog'),
        dialogHead = makeEl('dialog-head'),
        dialogBody = makeEl('dialog-body'),
        dialogFooter = makeEl('dialog-footer'),
        cross = makeEl('dialog-close');

    dialog.append(dialogHead).append(dialogBody).append(dialogFooter);
    dialogHead.append(cross);
    dialogHead.append(o.title);
    if (o.content) {
        dialogBody.append(o.content);
    }
    if (o.contentTemplate) {
        if (o.contentData) {
            dialogBody.append(mustache.render($(o.contentTemplate).text(), o.contentData));
        }
        if (o.sourceUrl) {
            overlay.addClass('loading');
            dialog.addClass('hidden');
            $.get(o.sourceUrl)
                .done(function(resp) {
                    overlay.removeClass('loading');
                    dialog.removeClass('hidden');
                    if (o.parseData) {
                        resp = o.parseData(resp);
                    }
                    if (dialog && dialog.length) {
                        dialogBody.append(mustache.render($(o.contentTemplate).text(), resp));
                        if (o.open) {
                            o.open();
                        }
                    }
                })
                .fail(function() {
                    uiAddons.showMessage('Ошибка сервера');
                });
        }
    }

    var close = function() {
        overlay.remove();
        dialog.remove();
        if (o.close) {
            o.close();
        }
    };

    $.each(o.actions, function(i, action) {
        var btn = $('<button>', {
            'text': action.name,
            'class': 'btn-m' + (action.primary ? '' : ' btn-light')
        });
        dialogFooter.append(btn);
        if (action.close) {
            btn.on('click', close);
        }
        if (action.onClick) {
            btn.on('click', function() {
                action.onClick(dialog);
            });
        }
    });

    if (o.className) {
        dialog.addClass(o.className);
    }

    $('body').append(overlay).append(dialog);

    cross.on('click', close);
    overlay.on('click', close);
    dialog.on('keypress', 'input', function(e) {
        if (e.keyCode == keyboard.ENTER) {
            o.onSubmit(dialog);
        }
    });

    return {
        close: close
    };
};

module.exports = dialog;