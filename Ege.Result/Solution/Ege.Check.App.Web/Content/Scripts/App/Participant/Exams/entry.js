(function() {

    var $ = require('jquery'),
        routes = require('./../../Config/routes.js'),
        startExamPage = require('./start-exam-pages.js'),
        buildExamsTable = require('./build-exams-table.js'),
        buildMessage = require('./build-staff-msg.js'),
        ui = require('./../../Utils/ui.js');

    startExamPage();

    var tableContainer = $('#table-container'),
        preloaderEl = tableContainer.find('.loader'),
        preloader = ui.showTablePreloader(preloaderEl);

    $.get(routes.getExams)
        .done(function(resp) {
            ui.hideTableProloader(preloaderEl, preloader);
            buildExamsTable(resp.Result.Exams);
            buildMessage(resp.Info);
        })
        .fail(function() {
            ui.showError(tableContainer, 'Ошибка сервера. Не удалось загрузить результаты экзаменов');
        });

})();