var gulp = require('gulp');
var config = require('../config');

gulp.task('watch', ['setWatch', 'setDebug', 'browserSync'], function() {
    gulp.watch(config.less.watch, ['less']);
    gulp.watch(config.html.watch, ['html']);
});