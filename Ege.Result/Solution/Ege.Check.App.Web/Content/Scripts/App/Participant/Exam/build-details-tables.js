var _ = require('underscore'),
    $ = require('jquery'),
    buildTable = require('./build-table.js'),
    buildPartATable = require('./build-part-a-table.js'),
    buildPartBTable = require('./build-part-b-table.js');

var buildTables = function(resp) {

    var answers = resp.Answers ? (resp.Answers.Answers || []) : [],
        info = resp.ExamInfo,
        tableContainer = $('#table-container');

    info.WithCriteria = _.sortBy(info.WithCriteria, function(item) {
        return item.Type;
    });

    var parts = _.groupBy(info.WithCriteria, function(item) {
        return item.Type;
    });

    var answersByParts = _.groupBy(answers, function(item) {
        return item.Type;
    });

    if (info.IsComposition && parts.hasOwnProperty('A') && parts.A.length) {
        tableContainer.append(buildPartATable(parts.A, answersByParts.A, resp.Answers.Mark5));
        $('#composition-notice').removeClass('hidden');
    }

    if (info.PartB && info.PartB.length) {
        tableContainer.append(buildPartBTable(info.PartB, answersByParts.B, info.IsBasicMath));
    }

    _.each(parts, function(part) {
        if (part.length && part[0].Type != 'A') {
            tableContainer.append(buildTable(part, answersByParts[part[0].Type], part[0].Type));
        }
    });

    if (!info.IsBasicMath && !info.IsComposition) {
        $('#notice-1, #notice-2').removeClass('hidden');
    }
    if (_.findWhere(answers, { Type: 'D' })) {
        $('#notice-3').removeClass('hidden');
    }
};

module.exports = buildTables;