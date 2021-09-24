var $ = require('jquery'),
    _ = require('underscore'),
    preloaderDelay = 200;

var showEntryFormMessage = function(msg, clear) {
    var container = $('#notice-container');

    if (clear) {
        container.empty();
    }

    container.append($('<div>', {
        'class': 'notice mb-15',
        'html': msg
    }));
};

var errMsg = 'Ошибка сервера',
    notValidMsg = 'Пожалуйста, заполните обязательные поля.',
    notValidCode = 'Некорректный код регистрации.<br/>Пожалуйста, проверьте правильность введённых данных.',
    authFailedMsg = 'Не удается войти.<br/>Пожалуйста, проверьте правильность введённых данных.';

var ui = {
    showBtnPreloader: function(button) {
        button.height(button.height());
        button.width(button.width());
        return setTimeout(function() {
            button.children().eq(0).addClass('hidden');
            button.children().eq(1).removeClass('hidden');
            button.prop('disabled', true);
        }, preloaderDelay);
    },

    hideBtnPreloader: function(button, timeout) {
        clearTimeout(timeout);
        button.children().eq(0).removeClass('hidden');
        button.children().eq(1).addClass('hidden');
        button.prop('disabled', false);
    },

    showTablePreloader: function(preloader) {
        return setTimeout(function() {
            preloader.removeClass('hidden');
        }, preloaderDelay);
    },

    hideTableProloader: function(preloader, timeout) {
        clearTimeout(timeout);
        if (preloader.length) {
            preloader.remove();
        }
    },

    showEntryFormMessages: {
        notValid: function(clear) {
            showEntryFormMessage(notValidMsg, clear);
        },
        notValidCode: function(clear) {
            showEntryFormMessage(notValidCode, clear);
        },
        authFailed: function(clear) {
            showEntryFormMessage(authFailedMsg, clear);
        }
    },

    showError: function(el, text) {
        text = text || errMsg;
        el.html($('<div>', {
            'class': 'notice',
            'text': text
        }));
    },

    setCaptcha: function(img) {
        $('#captcha-img').css('background', 'url(data:image/jpeg;base64,' + img + ')');
    },

    buildTable: function(opts) {
        var table = $('<table>', {
            'class': 'table'
        }),
            thead = $('<thead>'),
            tbody = $('<tbody>'),
            headRow = $('<tr>'),
            tfooter = null;

        if (opts.className) {
            table.addClass(opts.className);
        }

        _.each(opts.header, function(item) {
            headRow.append($('<th>', {
                'class': opts.name ? null : (item.className || null)
            }).html(item.label));
        });

        var renderOne = function(rowData, rowIndex) {
            var row = $('<tr>');
            if (rowIndex % 2 === 0) {
                row.addClass('odd');
            }
            _.each(opts.header, function(col, colIndex) {
                var cell = $('<td>');
                if (col.renderer) {
                    row.append(cell.append(col.renderer(rowData, rowData[col.name])));
                } else {
                    row.append(cell.html(rowData[col.name]));
                }
                if (col.cellClassName) {
                    cell.addClass(col.cellClassName);
                }
            });
            row.data('row', rowData);
            tbody.append(row);
        };

        _.each(opts.data, renderOne);

        if (opts.data.length === 0) {
            tbody.append($('<tr>').append($('<td>', {
                'colspan': opts.header.length,
                'text': 'Нет данных'
            })));
        }

        if (opts.name) {
            var colgroup = $('<colgroup>');
            _.each(opts.header, function(item) {
                colgroup.append($('<col>', {
                    'class': item.className || null
                }));
            });
            table.append(colgroup);
            thead.append($('<tr>').append($('<th>', {
                'text': opts.name,
                'colspan': opts.header.length
            })));
            headRow.addClass('sub-head');
        }

        if (opts.footer && opts.footer.length) {
            tfooter = $('<tfoot>');
            var footerRow = tfooter.append($('<tr>'));
            _.each(opts.footer, function(item) {
                var fCell = $('<td>');
                if (item.className) {
                    fCell.addClass(item.className);
                }
                if (item.colspan) {
                    fCell.attr('colspan', item.colspan);
                }
                if (item.renderer) {
                    fCell.text(item.renderer());
                }
                footerRow.append(fCell);
            });
        }

        table.append(thead.append(headRow)).append(tbody).append(tfooter);

        return {
            table: table,
            addRow: function(withData) {
                var length = tbody.find('tr').length;
                withData.Number = length + 1;
                renderOne(withData, length);
            },
            removeLastRow: function() {
                table.find('tbody tr:last').remove();
            }
        };
    }
};

module.exports = ui;