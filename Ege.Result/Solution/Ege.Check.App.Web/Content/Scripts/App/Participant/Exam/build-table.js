var _ = require('underscore'),
    $ = require('jquery'),
    ui = require('./../../Utils/ui.js');

var headers = {
    'C': 'Результаты выполнения заданий с развёрнутым ответом',
    'D': 'Результаты выполнения устной части'
};

var buildTable = function(info, answers, type) {

    if (!answers || !answers.length) {
        return null;
    }

    var data = [];

    var getMarkByNum = function(num) {
        if (answers) {
            var answer = _.findWhere(answers, { Number: num });
            if (answer) {
                return answer.Mark;
            }
        }
        return null;
    };

    var getMarkSumByCriteria = function(criteria) {
        if (answers) {
            return _.reduce(criteria, function(memo, curr) {
                var answer = _.findWhere(answers, { Number: curr.Number });
                if (answer) {
                    return memo + answer.Mark;
                }
                return memo;
            }, 0);
        }
        return null;
    };

    _.each(info, function(item, i) {
        if (item.Criteria && item.Criteria.length) {
            data.push({
                DisplayNumber: item.DisplayNumber,
                Mark: getMarkSumByCriteria(item.Criteria),
                MaxValue: _.reduce(item.Criteria, function(memo, curr) {
                    return _.isNumber(curr.MaxValue) ? memo + curr.MaxValue : memo;
                }, 0)
            });
            _.each(item.Criteria, function(c) {
                data.push({
                    Name: c.Name,
                    MaxValue: c.MaxValue,
                    Mark: getMarkByNum(c.Number)
                });
            });
        } else {
            data.push({
                DisplayNumber: item.DisplayNumber,
                MaxValue: item.MaxValue,
                Mark: getMarkByNum(item.Number)
            });
        }
    });

    var getSumRenderer = function(prop) {
        return function() {
            return _.reduce(data, function(memo, curr) {
                if (curr.DisplayNumber) {
                    return _.isNumber(curr[prop]) ? memo + curr[prop] : memo;
                }
                return memo;
            }, 0);
        };
    };

    var getMarkCellRenderer = function(prop) {
        return function(rowData) {
            if (rowData.DisplayNumber) {
                return $('<b>', { 'text': rowData[prop] });
            }
            return rowData[prop];
        };
    };

    _.each(data, function(item) {
        if (item.Name) {
            item.Name = item.Name.replace('***', '<span class="red">***</span>');
        }
    });

    var table = ui.buildTable({
        name: headers[type],
        data: data,
        className: 'mb-20',
        header: [{
                label: '№',
                name: 'DisplayNumber',
                className: 'width-60'
            }, {
                label: 'Критерии<span class="red">**</span>',
                name: 'Name',
                cellClassName: 'align-left'
            }, {
                label: 'Ваш балл<span class="red">*</span>',
                className: 'width-180',
                renderer: getMarkCellRenderer('Mark')
            }, {
                label: 'Максимальный балл<span class="red">*</span>',
                className: 'width-180',
                renderer: getMarkCellRenderer('MaxValue')
            }],
        footer: [{
                className: 'align-right',
                colspan: 2,
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

module.exports = buildTable;