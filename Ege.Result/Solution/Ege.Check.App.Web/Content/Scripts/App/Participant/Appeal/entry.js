(function() {

    var $ = require('jquery'),
        buildTable = require('./build-table.js'),
        routes = require('./../../Config/routes.js'),
        ui = require('./../../Utils/ui.js'),
        url = require('./../../Utils/url.js'),
        startExamPage = require('./../Exams/start-exam-pages.js');

    startExamPage();

    var tableContainer = $('#table-container'),
        preloaderEl = tableContainer.find('.loader'),
        preloader = ui.showTablePreloader(preloaderEl);

    $.get(routes.getAppeal(url.getLastPart()))
        .done(function(resp) {
            ui.hideTableProloader(preloaderEl, preloader);
            buildTable(resp);
        })
        .fail(function() {
            ui.showError(tableContainer, 'Ошибка сервера. Не удалось загрузить результаты экзамена');
        });

})();