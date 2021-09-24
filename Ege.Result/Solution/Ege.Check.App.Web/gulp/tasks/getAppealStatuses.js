var gulp = require('gulp'),
	sql = require('mssql'),
	gutil = require('gulp-util'),
	jeditor = require('gulp-json-editor'),
	configFile = require('../config.js'),
	sqlConfig = configFile.mssql,
	appealStatusesConfig = configFile.appealStatuses;

gulp.task('getAppealStatuses', function () {

	var query = 'SELECT * FROM AppealStatuses';

	var connection = new sql.Connection(sqlConfig, function (err) {
		if (err) {
			gutil.log(err);
		}

		var request = new sql.Request(connection);
		request.query(query, function (error, recordset) {
			if (error) {
				gutil.log(error);
			}
			console.log(appealStatusesConfig.folder + appealStatusesConfig.file);
			gulp.src(appealStatusesConfig.folder + appealStatusesConfig.file)	
				.pipe(jeditor(recordset))
				.pipe(gulp.dest(appealStatusesConfig.folder));
		});
	});

});
