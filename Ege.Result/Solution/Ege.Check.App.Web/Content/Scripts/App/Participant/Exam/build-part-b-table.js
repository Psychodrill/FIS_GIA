var _ = require('underscore'),
    $ = require('jquery'),
    ui = require('./../../Utils/ui.js');

var buildPartBTable = function(info, answers, isBasicMath) {

    var data = _.map(info, function(item) {
        var answer = _.findWhere(answers, { Number: item.Number });
        if (answer) {
            item.Answer = answer.Answer;
            item.Mark = answer.Mark;
        }
        return item;
    });

    var getSumRenderer = function(prop) {
        return function() {
            return _.reduce(data, function(memo, curr) {
                return _.isNumber(curr[prop]) ? memo + curr[prop] : memo;
            }, 0);
        };
    };

    var table = ui.buildTable({
        name: 'Результаты выполнения заданий с кратким ответом',
        className: 'mb-15',
        data: data,
        header: [{
                label: '№',
                name: 'Number',
                className: 'width-60'
            }, {
                label: 'Ваш ответ',
                name: 'Answer',
                cellClassName: 'align-left'
            }, {
                label: 'Допустимые символы',
                name: 'LegalSymbols',
                cellClassName: 'align-left'
            }, {
                label: 'Ваш балл' + (isBasicMath ? '' : '<span class="red">*</span>'),
                className: 'width-180',
                cellClassName: 'bold',
                renderer: function (rowData) {
                    if (rowData.Mark === 0 && rowData.MaxValue === 0) {
                        return '';
                    }
                    return rowData.Mark;
                }
            }, {
                label: 'Максимальный балл' + (isBasicMath ? '' : '<span class="red">*</span>'),
                className: 'width-180',
                cellClassName: 'bold',
                renderer: function (rowData) {
                    if (rowData.Mark === 0 && rowData.MaxValue === 0) {
                        return '';
                    }
                    return rowData.MaxValue;
                }
            }],
        footer: isBasicMath ? null : [{
                className: 'align-right',
                colspan: 3,
                renderer: function() {
                    return 'Итого';
                }
            }, {
                renderer: getSumRenderer('Mark')
            }, {
                renderer: getSumRenderer('MaxValue')
            }]
    }).table;

    return table;
};

module.exports = buildPartBTable;