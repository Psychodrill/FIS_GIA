var gulp = require('gulp');
var argv = require('yargs').argv;

gulp.task('setDebug', function () {
    global.debug = argv.debug || false;
});