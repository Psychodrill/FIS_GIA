var gulp = require('gulp'),
    sql = require('mssql'),
    gutil = require('gulp-util'),
    jeditor = require('gulp-json-editor'),
    configFile = require('../config.js'),
    sqlConfig = configFile.mssql,
    regionsConfig = configFile.regions;

gulp.task('getRegions', function() {

    var query = 'SELECT * FROM rbdc_Regions';

    var connection = new sql.Connection(sqlConfig, function(err) {
        if (err) {
            gutil.log(err);
        }

        var request = new sql.Request(connection);
        request.query(query, function(error, recordset) {
            if (error) {
                gutil.log(error);
            }
            gulp.src(regionsConfig.folder + regionsConfig.file)
                .pipe(jeditor(recordset))
                .pipe(gulp.dest(regionsConfig.folder));
        });
    });

});