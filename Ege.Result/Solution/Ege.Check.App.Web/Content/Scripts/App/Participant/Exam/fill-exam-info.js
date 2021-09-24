var $ = require('jquery'),
    _ = require('underscore'),
    mustache = require('mustache'),
    hasLocalStorage = require('./../../Utils/local-storage.js'),
    date = require('./../../Utils/date.js'),
    cond = require('../../Config/exam-conditions.js');

var blanksBlock = $('#blanks'),
    blanksWrap = blanksBlock.find('p'),
    blanksIsHiddenText = 'Возможность просмотра бланков по данному экзамену недоступна по решению регионального центра обработки информации (РЦОИ) субъекта РФ.';

var fillExamInfo = function(resp) {
    if (resp.Answers) {
        if (!resp.ExamInfo.IsComposition && !resp.ExamInfo.IsBasicMath) {
            $('#primary-mark').text(resp.Answers.PrimaryMark);
            $('#threshold').text(resp.ExamInfo.Threshold);
            $('#primary-mark-p, #threshold-p').removeClass('hidden');
        }

        if (resp.ExamInfo.IsComposition) {
            $('.c-test-mark-label').toggleClass('hidden');
        }

        $('#marks').removeClass('hidden');

        if (resp.GekDocument &&
            resp.GekDocument.GekNumber &&
            resp.GekDocument.GekDate &&
            !resp.ExamInfo.IsComposition) {

            $('#gek-number').text(resp.GekDocument.GekNumber);
            $('#gek-date').text(date.dateToRuFormat(resp.GekDocument.GekDate));
            $('#gek a').attr('href', resp.GekDocument.Url);
            $('#gek').removeClass('hidden');
        }

        var renderTestMark = function(text, className) {
            $('#test-mark').html($('<span>', {
                'text': text,
                'class': className
            }));
        };

        if (resp.ExamInfo.IsComposition) {
            renderTestMark(cond.composition.isPositive(resp.Answers.Mark5).string,
                cond.composition.isPositive(resp.Answers.Mark5).result ? 'green bold' : 'red bold');
        } else {
            renderTestMark(resp.Answers.TestMark,
                resp.Answers.TestMark >= resp.ExamInfo.Threshold ? 'green bold' : 'red bold');
        }

        if (hasLocalStorage) {
            $('#exam-name').text(localStorage.getItem('examname'));
            $('#exam-date').text(localStorage.getItem('examdate'));
        }

        if (resp.Answers.Blanks && resp.Answers.Blanks.length) {
            var server = resp.ExamInfo.IsComposition ?
                resp.Servers.Composition : resp.Servers.Common;

            _.each(resp.Answers.Blanks, function(item) {
                blanksWrap.append(mustache.render($('#blank-tmpl').text(), {
                    server: server,
                    blank: item
                }));
            });
            blanksBlock.removeClass('hidden');
        }

        if (resp.Answers.Blanks === null ||
            (resp.ExamInfo.IsComposition && (resp.Servers.Composition === '' || resp.Servers.Composition === null)) ||
            (!resp.ExamInfo.IsComposition && (resp.Servers.Common === '' || resp.Servers.Common === null))) {
            blanksWrap.text(blanksIsHiddenText);
            blanksBlock.removeClass('hidden');
        }
    }
};

module.exports = fillExamInfo;