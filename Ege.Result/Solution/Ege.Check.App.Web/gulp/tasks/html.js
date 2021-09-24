var gulp            = require('gulp');
var extender        = require('gulp-html-extend');
var uncache         = require('gulp-uncache');
var config          = require('../config').html;
var convertEncoding = require('gulp-convert-encoding');

gulp.task('html', function() {
    gulp.src(config.src)
        .pipe(extender({ annotations: true, verbose: false }))
        .pipe(uncache())
        .pipe(gulp.dest(config.dest));
});