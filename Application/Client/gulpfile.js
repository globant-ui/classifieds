/**
 * Created by alakule on 22-12-2016.
 */
var gulp = require('gulp'),
    gutil = require('gulp-util'),
    argv = require('yargs').argv,
    rename = require('gulp-rename'),
    chalk = require('chalk'),
    jsone = require("gulp-json-editor");;

gulp.task('default',function () {
  console.log('Hello Gulp!');
});

gulp.task('set-environment', function () {
  var env = (argv.production) ? 'prod' : (argv.test) ? 'qa' : 'dev';
  gutil.log(chalk.green('LOAD : ' + env + ' configuration.'));
  gulp.src("envoirnment_config/" + env + ".json")
    .pipe(jsone({'environment': env}))
    .pipe(rename("app/settings.json"))
    .pipe(gulp.dest("src/"));
     gulp.src('src/app/settings.json')
    .pipe(rename({dirname: ''}))
    .pipe(gulp.dest('__build__/'));
});
