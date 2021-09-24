(function() {

    var $ = require('jquery'),
        routes = require('./../../Config/routes.js'),
        startExamPage = require('./../Exams/start-exam-pages.js'),
        ui = require('./../../Utils/ui.js'),
        url = require('./../../Utils/url.js'),
        buildTables = require('./build-details-tables.js'),
        fillExamInfo = require('./fill-exam-info');

    startExamPage();

    var tableContainer = $('#table-container'),
        preloaderEl = tableContainer.find('.loader'),
        preloader = ui.showTablePreloader(preloaderEl);

    $.get(routes.getExam(url.getLastPart()))
        .done(function(resp) {
            ui.hideTableProloader(preloaderEl, preloader);
            fillExamInfo(resp);
            buildTables(resp);
        })
        .fail(function() {
            ui.showError(tableContainer, 'Ошибка сервера. Не удалось загрузить результаты экзамена');
        });

})();