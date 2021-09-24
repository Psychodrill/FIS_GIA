var $ = require('jquery'),
	_ = require('underscore'),
	ui = require('./../../Utils/ui.js'),
	dateUtils = require('./../../Utils/date.js'),
	routes = require('./../../Config/routes.js'),
	hasLocalStorage = require('./../../Utils/local-storage.js'),
	appealStatusesJSON = require('./../../Config/appeal-statuses.json'),
	cond = require('../../Config/exam-conditions.js');

var getStatusRenderer = function () {
	return function (rowData) {
	    if (!rowData.HasResult || rowData.IsHidden /*|| rowData.IsComposition && rowData.Mark5 != 5*/) {
			return '';
		}
		if (rowData.HasAppeal) {
			var status = _.findWhere(appealStatusesJSON, { Status: rowData.AppealStatus });
			if (status) {
				return $('<a>', {
					'href': routes.appealPage(rowData.ExamId),
					'text': status.Description
				});
			}
		}
		return 'нет';
	};
};

var hasOralExam = function (exam) {
	if (!exam.OralExamId && !exam.OralExamDate && !exam.OralStatus && !exam.OralSubject) {
		return false;
	}
	return true;
};

var rowspanCallback = function (rowData) {
	if (rowData.IsForeignLanguage && rowData.rowspan) {
		return 2;
	}
};

var blankCallback = function (rowData) {
	if (rowData.IsForeignLanguage && rowData.blank) {
		return true;
	}
};

var buildExamsTable = function (data) {

	var _data = _.reduce(data, function (memo, curr) {
		if (curr.IsForeignLanguage && hasOralExam(curr)) {
			var oralExam = _.clone(curr);
			oralExam.isOralExam = true;
			oralExam.ExamDate = curr.OralExamDate;
			oralExam.Status = curr.OralStatus;
			curr.OralSubject = curr.Subject + ' (письменный)';

			var addToMemo = function (first, sec) {
				first.rowspan = true;
				sec.blank = true;
				memo.push(first);
				memo.push(sec);
			};
			if ((new Date(curr.ExamDate)).getTime() > (new Date(curr.OralExamDate)).getTime()) {
				addToMemo(oralExam, curr);
			} else {
				addToMemo(curr, oralExam);
			}
		} else {
			memo.push(curr);
		}
		return memo;
	}, []);

	var table = ui.buildTable({
		header: [{
			label: 'Дата экзамена',
			className: 'width-110',
			renderer: function (rowData) {
				return dateUtils.dateToRuFormat(rowData.ExamDate);
			}
		}, {
			label: 'Предмет',
			renderer: function (rowData) {
			    if (rowData.IsHidden || !rowData.HasResult) {
					return rowData.OralSubject || rowData.Subject; 
				}
				return $('<a>', {
					'href': routes.examPage(rowData.ExamId),
					text: rowData.OralSubject || rowData.Subject
				}).click(function (e) {
					if (hasLocalStorage) {
						localStorage.setItem('examname', rowData.Subject);
						if (rowData.OralExamDate) {
							localStorage.removeItem('examdate');	
						} else {
							localStorage.setItem('examdate', dateUtils.dateToRuFormat(rowData.ExamDate));
						}
					}
				});
			}
		}, {
			label: 'Тестовый балл',
			className: 'width-110',
			renderer: function (rowData) {
				if (rowData.IsHidden || !rowData.HasResult) {
					return '';
				}
				var mark = rowData.TestMark;
				if (rowData.IsComposition) {
					mark = rowData.Mark5 == 5 ? 'зачёт' : 'незачёт';
				}
				var span = $('<span>', {
					text: mark
				});

				if ((!rowData.IsComposition && rowData.TestMark >= rowData.MinMark) || 
					(rowData.IsComposition && rowData.Mark5 == cond.composition.minMark)) {
					span.addClass('bold green');
				} else {
					span.addClass('bold red');
				}
				return span;
			},
			rowspan: rowspanCallback,
			blank: blankCallback
		}, {
			label: 'Минимальный балл',
			className: 'width-110',
			renderer: function (rowData) {
				if (rowData.IsHidden || !rowData.HasResult || rowData.IsComposition) {
					return '';
				}
				if (rowData.IsBasicMath) {
					return cond.basicMath.minMark;
				}
				return rowData.MinMark;
			},
			rowspan: rowspanCallback,
			blank: blankCallback
		}, {
			label: 'Статус экзамена',
			name: 'Status',
			renderer: function (rowData) {
			    //				var hasResult = rowData.isOralExam ? rowData.HasOralResult : rowData.HasResult;
                
			    var hasResult = rowData.HasResult;
				if (!hasResult) {
					return 'Нет результата';
				}
                if (hasResult && rowData.IsComposition && rowData.Mark5 != 5) {
                    return 'Нет результата со значением &laquo;зачёт&raquo;';
                }
                if (hasResult && rowData.IsComposition && rowData.Mark5 == 5) {
			        return '&laquo;Зачёт&raquo;';
			    }
				if (hasResult && !rowData.IsHidden) {
					return 'Экзамен обработан';
				}
				return 'Результат скрыт';
			}
		}, {
			label: 'Апелляция',
			className: 'width-110',
			renderer: getStatusRenderer(),
			rowspan: rowspanCallback,
			blank: blankCallback
		}],
		className: 'mb-20',
		data: _data
	}).table;

	$('#table-container').html(table);
};

module.exports = buildExamsTable;