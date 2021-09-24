var $ = require('jquery'),
    _ = require('underscore'),
    mustache = require('mustache'),
    isFct = require('./../is-fct.js'),
    routes = require('../../Config/staffRoutes.js'),
    regionsJSON = require('../../Config/regions.json'),
    uiAddons = require('../../Utils/ui-addons.js'),
    date = require('../../Utils/date.js');

var data,
    dateSelect = $('#date'),
    examSelect = $('#exam');

var drawExamsSelect = function(date) {
    var exams = _.findWhere(data, { Date: date }).Exams;
    examSelect.empty();
    _.each(exams, function(item) {
        examSelect.append($('<option>', {
            'text': item.SubjectName,
            'value': item.ExamGlobalId
        }));
    });
};

var drawDatesSelect = function() {
    _.each(data, function(item) {
        dateSelect.append($('<option>', {
            'text': date.dateToRuFormat(item.Date),
            'value': item.Date
        }));
    });
    if (data && data.length && data[0].Date) {
        drawExamsSelect(data[0].Date);
    }
};

var initForm = function() {
    $.get(routes.getExamList)
        .done(function(resp) {
            data = resp;
            drawDatesSelect();
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось загрузить список экзаменов');
        });

    dateSelect.change(function(e) {
        drawExamsSelect($(e.target).val());
    });

    if (isFct()) {
        $('#region-block')
            .append(mustache.render($('#region-tmpl').text(), {
                regions: regionsJSON
            }))
            .removeClass('hidden');
    }
};

module.exports = initForm;