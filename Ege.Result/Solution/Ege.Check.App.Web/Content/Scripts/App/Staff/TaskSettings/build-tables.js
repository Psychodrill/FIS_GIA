var $            = require('jquery');
var _            = require('underscore');
var mustache     = require('mustache');
var renderPartA  = require('./render-part-a.js');
var renderPartB  = require('./render-part-b.js');
var renderPartCD = require('./render-part-c-d.js');
var save         = require('./save.js');
var routes       = require('../../Config/staffRoutes.js');

var tableContainer  = $('#table-container');
var buttonTemplate  = $('#table-buttons-tmpl').text();
var saveBtnTemplate = $('#save-tmpl').text();
var criteria        = [];
var partB;
var otherParts;
var isComposition;

var render = function(resp) {
    if (resp.IsComposition) {
        otherParts = renderPartA(resp.WithCriteria && resp.WithCriteria[0] && resp.WithCriteria[0].Criteria ? resp.WithCriteria[0].Criteria : [], buttonTemplate);
        tableContainer.append(otherParts);
        partB = null;
    } else {
        partB = renderPartB(resp.PartB || [], buttonTemplate);
        tableContainer.append(partB);
        otherParts = $('<div>');

        if (!resp.IsBasicMath) {
            otherParts.append(renderPartCD(_.where(resp.WithCriteria, { Type: 'C' }), buttonTemplate, 'C'));
        }

        if (resp.IsForeignLanguage) {
            otherParts.append(renderPartCD(_.where(resp.WithCriteria, { Type: 'D' }), buttonTemplate, 'D'));
        }

        tableContainer.append(otherParts);
    }

    tableContainer.append(mustache.render(saveBtnTemplate));
    isComposition = resp.IsComposition;

};

var buildTables = function(e) {
    var target = $(e.target);

    tableContainer.empty();
    var preloader = setTimeout(function() {
        tableContainer.addClass('loading');
    }, 200);

    $.get(routes.taskSettings(target.val()))
        .done(render)
        .fail()
        .always(function() {
            clearTimeout(preloader);
            tableContainer.removeClass('loading');
        });

    tableContainer.off().on('click', '#save', function() {
        save(partB, otherParts, target.val(), isComposition);
    });
};

module.exports = buildTables;