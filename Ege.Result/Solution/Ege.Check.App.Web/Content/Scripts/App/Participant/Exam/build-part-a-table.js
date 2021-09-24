var $ = require('jquery'),
	_ = require('underscore'),
	ui = require('./../../Utils/ui.js'),
	cond = require('../../Config/exam-conditions.js');

var buildTable = function (info, answers, mark5) {
	
	if (!answers || !answers.length || !info || !info[0] || !info[0].Criteria) {
		return null;
	}

	var data = [];
	
	_.each(info[0].Criteria, function (item, i) {
		var mark = _.findWhere(answers, { Number: item.Number }); 
		data.push({
			Number: item.Number,
			Name: item.Name,
			Mark: mark ? mark.Mark : 0
		});
	});

	var table = ui.buildTable({
		name: 'Результат по критериям',
		data: data,
		className: 'mb-20',
		header: [{
			label: '№',
			name: 'Number',
			className: 'width-60'
		}, {
			label: 'Название критерия',
			name: 'Name',
			cellClassName: 'align-left'
		}, {
			label: 'Результат (зачёт)',
			className: 'width-180',
			renderer: function (rd) {
				return $('<span>', {
					'text': rd.Mark > 0 ? '+' : '-',
					'class': rd.Mark > 0 ? 'bold green' : 'bold red'
				});
			}
		}],
		footer: [{
			colspan: 3,
			className: 'align-right',
			renderer: function () {
				return 'Итог: ' + cond.composition.isPositive(mark5).string;
			}
		}]
	}).table;

	return table;
};

module.exports = buildTable;