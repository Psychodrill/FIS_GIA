var $                = require('jquery');
var _ = require('underscore');

//var dataTable = require('datatables');
var dataTable = require('datatables')(window, $);
var isFct            = require('./../is-fct.js');
var routes           = require('../../Config/staffRoutes.js');
var dateHelper       = require('../../Utils/date.js');
var ui               = require('../../Utils/ui.js');
var uiAddons         = require('../../Utils/ui-addons.js');
var dtLanguage       = require('../../Utils/dataTable-ru.js');
var editGekDocument  = require('./edit-gek-document.js');
var toggleCheckboxes = require('./toggle-checkboxes.js');

var table;
var tablePreloader;
var changes        = [];
var waveSelect     = $('#wave');
var saveBtn        = $('.c-save');
var tableSelector  = '#settings-table';
var tableContainer = $('#table-container');

var getSelectedRegion = function() {
    return $('#regions').val();
};

var save = function() {
    var preloader = ui.showBtnPreloader(saveBtn);
    var url = isFct() ? routes.setSettings(getSelectedRegion()) : routes.setSettingsForMyRegion;

    $.post(url, { settings: changes })
        .done(function() {
            uiAddons.showMessage('Изменения успешно сохранены', true);
        })
        .fail(function() {
            uiAddons.showMessage('Ошибка сервера. Не удалось сохранить изменения');
        })
        .always(function() {
            ui.hideBtnPreloader(saveBtn, preloader);
        });

    changes = [];
};

var setChanges = function(e) {
    var target = $(e.target),
        cell = target.parents('td'),
        row = target.parents('tr'),
        data = table.row(row.get(0)).data(),
        cellProp = table.column(table.cell(cell).index().column).dataSrc(),
        item = _.findWhere(changes, { ExamGlobalId: data.ExamGlobalId });

    if (!item) {
        item = _.clone(data);
        changes.push(item);
    }
    item[cellProp] = target.prop('checked');
    saveBtn.prop('disabled', !changes.length);
};

var dateColRender = function(data, type, full, meta) {
    if (full.IsComposition) {
        return 'все';
    }
    return dateHelper.dateToRuFormat(data);
};

var checkboxColRender = function(data, type, full, meta) {
    return $('<input>', {
        'type': 'checkbox',
        'checked': data
    }).get(0).outerHTML;
};

var gekColRenderer = function(data, type, full, meta) {
    if (full.IsComposition) {
        return '';
    }
    return $('<a>', {
        'href': '#',
        'text': data ? 'есть' : 'нет',
        'class': 'c-gek'
    }).get(0).outerHTML;
};

var initDataTable = function() {
    var url = isFct() ?
        routes.getSettings(getSelectedRegion()) :
        routes.getSettingsForMyRegion;

    table = $(tableSelector).DataTable({
        ajax: {
            url: url,
            data: function(d) {
                d.wave = waveSelect.val();
            },
            dataSrc: function(data) {
                return data.Settings;
            }
        },
        columns: [
            { data: 'SubjectName' },
            { data: 'ExamDate', render: dateColRender, orderData: [2] },
            { data: 'ExamDate', visible: false },
            { data: 'ShowResult', render: checkboxColRender, orderable: false },
            { data: 'ShowBlank', render: checkboxColRender, orderable: false },
            { data: 'HasGekDocument', render: gekColRenderer, orderable: false }
        ],
        paging: false,
        searching: false,
        autoWidth: false,
        info: false,
        order: [[1, 'asc']],
        language: dtLanguage
    });

    table
        .on('preXhr', function() {
            tablePreloader = setTimeout(function() {
                tableContainer.addClass('loading');
            }, 200);
        })
        .on('xhr', function() {
            clearTimeout(tablePreloader);
            tableContainer.removeClass('loading');
        });

    return table;
};

var buildTable = function() {
    if (!isFct()) {
        initDataTable();
    }

    waveSelect.on('change', function() {
        table.ajax.reload();
    });

    saveBtn.on('click', save);

    $(tableSelector)
        .on('change', 'input[type="checkbox"]', setChanges)
        .on('click', '.c-gek', function(e) {
            e.preventDefault();
            var row = $(e.target).parents('tr');
            var cell = $(e.target).parents('td');
            editGekDocument(
                table.row(row.get(0)).data().ExamGlobalId,
                table.cells(cell.get(0)),
                function(value) {
                    table.cell(cell.get(0)).data(value).draw();
                },
                getSelectedRegion);
        })
        .on('click', '.c-check-all', function(e) {
            e.preventDefault();
            toggleCheckboxes($(e.target), true);
        })
        .on('click', '.c-uncheck-all', function(e) {
            e.preventDefault();
            toggleCheckboxes($(e.target), false);
        });

    return initDataTable;
};

module.exports = buildTable;